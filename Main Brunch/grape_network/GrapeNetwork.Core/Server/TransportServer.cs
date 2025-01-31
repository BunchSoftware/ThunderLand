﻿using GrapeNetwork.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace GrapeNetwork.Core.Server
{
    public class TransportServer : IDisposable
    {
        private Socket TcpSocketServer;

        private IPEndPoint IPEndPoint;
        public string RemoteAdress => IPEndPoint.ToString();

        // Список подключенных
        public List<Connection> Connections { get; } = new List<Connection>();

        // События для общения с обработчиком
        public event Action<Connection> OnConnectedClient;
        public event Action<Connection> OnDisconnectedClient;
        public event Action<Connection, Package> OnRecieveDataClient;
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnDebugInfo;

        public int MaxCountConnection = 0;
        public int SendBufferSize = 64 * 1024;
        public int RecievedBufferSize = 64 * 1024;

        private bool isActive = false;
        public bool IsActive { get => isActive; }

        public TransportServer(int Port)
        {
            IPEndPoint = new IPEndPoint(Dns.GetHostByName(Dns.GetHostName()).AddressList[0], Port);
        }

        public TransportServer(int Port, IPAddress IPAddress)
        {
            IPEndPoint = new IPEndPoint(IPAddress, Port);
        }

        public TransportServer(IPEndPoint IPEndPoint)
        {
            this.IPEndPoint = IPEndPoint;
        }

        ~TransportServer()
        {
            Dispose();
        }

        // Принятие новых пользователей для подключения
        private void AcceptCallback(IAsyncResult asyncResult)
        {
            if (MaxCountConnection == 0 || Connections.Count >= MaxCountConnection && isActive)
            {
                try
                {
                    Connection connection = new Connection(TcpSocketServer.EndAccept(asyncResult), SendBufferSize, RecievedBufferSize);
                    Connections.Add(connection);

                    connection.OnDisconnect += () =>
                    {
                        DisconnectedClient(connection);
                    };

                    connection.WorkSocket.BeginReceive(connection.ReceiveBuffer, 0, connection.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), connection);
                    TcpSocketServer.BeginAccept(new AsyncCallback(AcceptCallback), null);
                    OnDebugInfo?.Invoke($"Подключен клиент по адресу {connection.RemoteAdressClient}");
                    OnConnectedClient?.Invoke(connection);
                }
                catch (ObjectDisposedException) { }
            }
        }
        // Обработка потока данных от пользователя
        private void ProcessInputData(Connection connection)
        {
            TransportProtocol transportProtocol = connection.transportProtocol;
            while (transportProtocol.RecievePackageCount > 0)
            {
                // Преобразуем пакет из массива байт в JSON формат
                Package package = transportProtocol.GetLastPackage();
                OnRecieveDataClient?.Invoke(connection, package);
            }
        }
        // Принимаем сообщение от клиента
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            Connection connection = (Connection)asyncResult.AsyncState;
            try
            {
                if (connection.IsConnected)
                {
                    int received = connection.WorkSocket.EndReceive(asyncResult);
                    // Читаем поток данных от клиента
                    byte[] data = new byte[received];
                    Array.Copy(connection.ReceiveBuffer, data, received);
                    if (received != 0)
                        connection.transportProtocol.CreatePackage(data);

                    // Отправляем на обработку данных
                    ProcessInputData(connection);

                    connection.WorkSocket.BeginReceive(connection.ReceiveBuffer, 0, connection.ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, connection);
                }
            }
            catch (Exception ex) { OnExceptionInfo?.Invoke(ex); }
        }

        // Отправка пакета данных
        public void SendPackage(Connection connection, Package package)
        {
            IPEndPoint IPEndPoint = connection.WorkSocket.RemoteEndPoint as IPEndPoint;
            if (IPEndPoint != null)
            {
                package.IPConnection = Package.ConvertFromIpAddressToInteger(IPEndPoint.Address.ToString());
                byte[] encodedPackage = connection.transportProtocol.CreateBinaryData(package);
                if (package.AuthAndGetRSAKey)
                    OnDebugInfo?.Invoke($"Клиенту {connection.RemoteAdressClient} был отправлен RSA Key");
                if (connection != null)
                    connection.WorkSocket.BeginSend(encodedPackage, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), connection);
            }
        }

        // Отправка пакета данных
        public void SendPackage(string localPointClient, Package package)
        {
            Connection connection = Connections.Find(connection => connection.RemoteAdressClient == localPointClient);
            SendPackage(connection, package);
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            Connection connection = (Connection)asyncResult.AsyncState;
            connection.WorkSocket.EndSend(asyncResult);
        }

        private void DisconnectedClient(Connection connection)
        {
            OnDebugInfo?.Invoke($"Клиент под адресом {connection.RemoteAdressClient} отключен от сервера");
            Connections.RemoveAt((int)connection.IDConnection - 1);
            OnDisconnectedClient?.Invoke(connection);
        }
        // Запуск сервера
        public bool Start()
        {
            try
            {
                if (isActive == false)
                {
                    TcpSocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    TcpSocketServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                    TcpSocketServer.Bind(IPEndPoint);
                    TcpSocketServer.Listen(10);
                    TcpSocketServer.BeginAccept(AcceptCallback, null);
                    isActive = true;
                    OnDebugInfo?.Invoke("Сервер запущен");
                    return true;
                }
                OnDebugInfo("Сервер уже запущен");
                return false;
            }
            catch (SocketException ex)
            {
                OnExceptionInfo?.Invoke(ex);
                OnDebugInfo?.Invoke("Пожалуйста, освободите занятый сетевой порт или попробуйте другой");
                return false;
            }
        }

        public void Stop()
        {
            try
            {
                if (isActive == true)
                {
                    for (int i = 0; i < Connections.Count; i++)
                    {
                        Package package = new Package();
                        package.Shutdown = true;

                        if (Connections[i].IsConnected)
                        {
                            OnDebugInfo?.Invoke($"Клиент под адресом {Connections[i].RemoteAdressClient} был отключен от сервера");
                            SendPackage(Connections[i], package);
                            DisconnectedClient(Connections[i]);
                        }
                    }
                    TcpSocketServer.Close();
                    TcpSocketServer.Dispose();
                    OnDebugInfo?.Invoke("Сервер отключен");
                    isActive = false;

                    OnConnectedClient = null;
                    OnDisconnectedClient = null;
                    OnRecieveDataClient = null;
                    OnExceptionInfo = null;
                    OnDebugInfo = null;
                }
            }
            catch (Exception ex) { OnExceptionInfo?.Invoke(ex); }
        }

        public void Stop(int timeout)
        {
            Task.Factory.StartNew(() => { Thread.Sleep(timeout); Stop(); });
        }

        public void Dispose()
        {
            Stop();
        }
    }
}