using GrapeNetwork.Packages;
using GrapNetwork.LogWriter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace GrapNetwork.Client
{
    public class NetworkClient : ICommandClient, IDisposable
    {
        public int IDClient;

        private Socket TcpSocketClient;

        private ConnectionState _ConnectionState = ConnectionState.Disconnected;
        public ConnectionState ConnectionState { get => _ConnectionState; }

        public bool IsConnected
        {
            get
            {
                try
                {
                    if (TcpSocketClient != null && TcpSocketClient.Connected)
                    {
                        if (TcpSocketClient.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (TcpSocketClient.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                OnDisconnectEvent?.Invoke();
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        OnDisconnectEvent?.Invoke();
                        return false;
                    }
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

        public event Action OnConnectEvent;
        public event Action OnDisconnectEvent;
        public event Action OnServerShutdown;
        public event Action<Package> OnSendDataEvent;
        public event Action<Package> OnRecieveDataEvent;
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnDebugInfo;

        private List<PackageProcessingCondition> packageProcessingConditions = new List<PackageProcessingCondition>();

        private int CountOfNegatives = 5; // Количество попыток подключений
        private int ReconnectionTime = 100;
        public string LocalAdressClient => TcpSocketClient.LocalEndPoint.ToString();

        public int SendBufferSize = 64 * 1024;
        public int RecievedBufferSize = 64 * 1024;


        public byte[] SendBuffer;
        public byte[] ReceiveBuffer;

        private StreamPackager streamPackager = new StreamPackager();

        public NetworkClient(List<PackageProcessingCondition> packageProcessingConditions)
        {
            TcpSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpSocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.packageProcessingConditions = packageProcessingConditions;
            TcpSocketClient.ReceiveBufferSize = RecievedBufferSize;
            TcpSocketClient.SendBufferSize = SendBufferSize;
            ReceiveBuffer = new byte[RecievedBufferSize];
            SendBuffer = new byte[SendBufferSize];
        }

        public NetworkClient()
        {
            TcpSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpSocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            TcpSocketClient.ReceiveBufferSize = RecievedBufferSize;
            TcpSocketClient.SendBufferSize = SendBufferSize;
            ReceiveBuffer = new byte[RecievedBufferSize];
            SendBuffer = new byte[SendBufferSize];
        }

        ~NetworkClient()
        {
            Dispose();
        }

        public void ConnectToServer(int portServer, IPAddress IPAddressServer)
        {
            _ConnectionState = ConnectionState.Waiting;
            for (int i = 0; i < CountOfNegatives & !IsConnected; i++)
            {
                    try
                    {
                        TcpSocketClient.Connect(IPAddressServer, portServer);
                        _ConnectionState = ConnectionState.Connected;
                        OnConnectEvent?.Invoke();
                        OnDebugInfo?.Invoke($"Произошло подключение к серверу {IPAddressServer}:{portServer}");
                        TcpSocketClient.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
                        Thread.Sleep(ReconnectionTime);
                    }
                    catch (Exception ex)
                    {
                        OnExceptionInfo?.Invoke(ex);
                    }
            }
        }
        public void DisconnectFromServer()
        {
            if (IsConnected)
            {
                try
                {
                    _ConnectionState = ConnectionState.Disconnected;
                    OnDisconnectEvent?.Invoke();
                    OnDebugInfo?.Invoke($"Произошло отключение от сервера {LocalAdressClient}");
                    TcpSocketClient.Shutdown(SocketShutdown.Both);
                    TcpSocketClient.Close();
                    TcpSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                catch (Exception ex)
                {
                    OnExceptionInfo?.Invoke(ex);
                }
            }
        }
        private void ProcessInputData()
        {
            while (streamPackager.PackagerCount > 0)
            {
                string data = Encoding.UTF8.GetString(streamPackager.Read());
                Package package = JsonConvert.DeserializeObject<Package>(data);

                if (package.Headers.Connection == "Disconnect" && package.StatusCode == StatusCode.OK && package.ContentType == "ServerStop")
                {
                    OnServerShutdown?.Invoke();
                    OnDebugInfo?.Invoke($"Был выключен сервер {LocalAdressClient}");
                    DisconnectFromServer();
                    return;
                }
                else if (packageProcessingConditions.Count > 0)
                {
                    foreach (var condition in packageProcessingConditions)
                    {
                        if (condition.CheckCondition(package))
                            OnRecieveDataEvent?.Invoke(package);
                    }
                    return;
                }
                OnRecieveDataEvent?.Invoke(package);
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            if (TcpSocketClient.Connected)
            {
                try
                {
                    if (TcpSocketClient.Connected)
                    {
                        int received = TcpSocketClient.EndReceive(asyncResult);
                        if (received != 0)
                            streamPackager.Write(ReceiveBuffer, received);

                        ProcessInputData();

                        TcpSocketClient.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
                    }
                }
                catch (Exception ex) { OnExceptionInfo?.Invoke(ex); }
            }
        }

        public void SetCondition(List<PackageProcessingCondition> packageProcessingConditions)
        {
            this.packageProcessingConditions = packageProcessingConditions;
        }

        private void SendData(Package package)
        {
            if (IsConnected)
            {
                byte[] encodedPackage = PackageBuilder.ToByteArray(package);
                Array.Copy(encodedPackage, SendBuffer, encodedPackage.Length);
                TcpSocketClient.BeginSend(SendBuffer, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            }
        }

        public void SendPackage(Package package, bool isCallSendEvent)
        {
            SendData(package);
            if (isCallSendEvent)
               OnSendDataEvent?.Invoke(package);
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            TcpSocketClient.EndSend(asyncResult);
        }


        public void Dispose()
        {
            DisconnectFromServer();
        }
    }
}
