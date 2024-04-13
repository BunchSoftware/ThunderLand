using GrapeNetwork.Packages;
using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GrapeNetwork.Core.Server;

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

        // Кондиции для обработки пакетов данных от клиентов
        private List<PackageProcessingCondition> packageProcessingConditions = new List<PackageProcessingCondition>();

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
                    connection.IDConnection = Connections.Count + 1;
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

                if (packageProcessingConditions.Count > 0)
                {
                    // Проверяем пакет данных на соответсвие кондициям
                    foreach (var condition in packageProcessingConditions)
                    {
                        if (condition.CheckCondition(package))
                        {
                            OnRecieveDataClient?.Invoke(connection, package);
                            break;
                        }
                    }
                }
                else
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

        // Задаем кондиции для обработки пакетов
        public void SetCondition(List<PackageProcessingCondition> packageProcessingConditions)
        {
            this.packageProcessingConditions = packageProcessingConditions;
        }

        // Отправка пакета данных
        public void SendPackage(Connection connection, Package package)
        {
            byte[] encodedPackage = connection.transportProtocol.CreateBinaryData(package);
            if (package.AuthAndGetRSAKey)
                OnDebugInfo?.Invoke($"Клиент {connection.RemoteAdressClient} авторизован");
            connection.WorkSocket.BeginSend(encodedPackage, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), connection);
        }

        // Отправка пакета данных
        public void SendPackage(int IDConnection, Package package)
        {
            Connection connection = Connections.Find(connection => connection.IDConnection == IDConnection);
            byte[] encodedPackage = connection.transportProtocol.CreateBinaryData(package);
            if (package.AuthAndGetRSAKey)
                OnDebugInfo?.Invoke($"Клиент {connection.RemoteAdressClient} получил RSA Key");
            if (connection != null)
                connection.WorkSocket.BeginSend(encodedPackage, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), connection);
        }

        // Отправка пакета данных
        public void SendPackage(string localPointClient, Package package)
        {
            Connection connection = Connections.Find(connection => connection.RemoteAdressClient == localPointClient);
            byte[] encodedPackage = connection.transportProtocol.CreateBinaryData(package);
            if (package.AuthAndGetRSAKey)
                OnDebugInfo?.Invoke($"Клиенту {connection.RemoteAdressClient} был отправлен RSA Key");
            if (connection != null)
                connection.WorkSocket.BeginSend(encodedPackage, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), connection);
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            Connection connection = (Connection)asyncResult.AsyncState;
            connection.WorkSocket.EndSend(asyncResult);
        }

        private void DisconnectedClient(Connection connection)
        {
            OnDebugInfo?.Invoke($"Клиент под адресом {connection.RemoteAdressClient} отключен от сервера");
            Connections.RemoveAt(connection.IDConnection - 1);
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