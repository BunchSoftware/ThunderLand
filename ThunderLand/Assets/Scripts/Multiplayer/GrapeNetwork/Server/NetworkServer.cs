using GrapeNetwork;
using GrapeNetwork.Packages;
using GrapNetwork.Client;
using GrapNetwork.LogWriter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrapNetwork.Server
{
    public class NetworkServer : ICommandServer, IDisposable
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

        public NetworkServer(int Port)
        {
            IPEndPoint = new IPEndPoint(Dns.GetHostByName(Dns.GetHostName()).AddressList[0], Port);
        }

        public NetworkServer(int Port, IPAddress IPAddress)
        {
            IPEndPoint = new IPEndPoint(IPAddress, Port);
        }

        public NetworkServer(IPEndPoint IPEndPoint)
        {
            this.IPEndPoint = IPEndPoint;
        }

        ~NetworkServer()
        {
            Dispose();
        }

        // Принятие новых пользователей для подключения
        private void AcceptCallback(IAsyncResult asyncResult)
        {
            if (MaxCountConnection == 0 || Connections.Count >= MaxCountConnection && isActive)
            {
                Connection connection = new Connection(TcpSocketServer.EndAccept(asyncResult), SendBufferSize, RecievedBufferSize);
                connection.IDConnection = Connections.Count + 1;
                Connections.Add(connection);

                connection.OnDisconnect += () =>
                {
                    DisconnectedClient(connection);
                };

                connection.WorkSocket.BeginReceive(connection.ReceiveBuffer, 0, connection.ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), connection);
                OnConnectedClient?.Invoke(connection);
                TcpSocketServer.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
        }
        // Обработка потока данных от пользователя
        private void ProcessInputData(Connection connection) 
        {
            StreamPackager streamPackager = connection.streamPackager;
            while (streamPackager.PackagerCount > 0)
            {
                // Преобразуем пакет из массива байт в JSON формат
                string data = Encoding.UTF8.GetString(streamPackager.Read());
                Package package = JsonConvert.DeserializeObject<Package>(data);

                // Проверяем, что это пакет на отключение от сервера
                if (package.ContentType == "Disconnect" && package.StatusCode == StatusCode.OK && package.Headers.Connection == "Disconnect")
                {
                    DisconnectedClient(connection);
                    OnDebugInfo?.Invoke($"Клиент под адресом {connection.RemoteAdressClient} отключен от сервера от сервера");
                    return;
                }
                else if(packageProcessingConditions.Count > 0)
                {
                    // Проверяем пакет данных на соответсвие кондициям
                    foreach (var condition in packageProcessingConditions)
                    {
                        if (condition.CheckCondition(package))
                        {
                            OnRecieveDataClient?.Invoke(connection, package);
                        }
                    }
                    return;
                }
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
                    if (received != 0)
                        connection.streamPackager.Write(connection.ReceiveBuffer, received);

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
            byte[] encodedPackage = PackageBuilder.ToByteArray(package);
            connection.WorkSocket.BeginSend(encodedPackage, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), connection);
        }

        // Отправка пакета данных
        public void SendPackage(int IDConnection, Package package)
        {
            byte[] encodedPackage = PackageBuilder.ToByteArray(package);
            Connection connection = Connections.Find(connection => connection.IDConnection == IDConnection);
            if (connection != null)
                connection.WorkSocket.BeginSend(encodedPackage, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), connection);
        }

        // Отправка пакета данных
        public void SendPackage(string localPointClient, Package package)
        {
            byte[] encodedPackage = PackageBuilder.ToByteArray(package);
            Connection connection = Connections.Find(connection => connection.RemoteAdressClient == localPointClient);
            if(connection != null)
                connection.WorkSocket.BeginSend(encodedPackage, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), connection);
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            Connection connection = (Connection)asyncResult.AsyncState;
            connection.WorkSocket.EndSend(asyncResult);
        }

        private void DisconnectedClient(Connection connection)
        {
            connection.Close();
            Connections.Remove(connection);
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
                        package.StatusCode = StatusCode.OK;
                        package.ContentType = "ServerStop";
                        package.Headers.Connection = "Disconnect";

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
            catch(Exception ex) { OnExceptionInfo?.Invoke(ex); }
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