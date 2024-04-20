using GrapeNetwork;
using GrapeNetwork.Packages;
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


namespace GrapeNetwork.Core.Client
{
    public class TransportClient : IDisposable
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
                catch (Exception)
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
        public event Action OnAuthorized;

        private int CountOfNegatives = 5; // Количество попыток подключений
        private int ReconnectionTime = 100;
        public string LocalAdressClient => TcpSocketClient.LocalEndPoint.ToString();

        public int SendBufferSize = 64 * 1024;
        public int RecievedBufferSize = 64 * 1024;

        public byte[] SendBuffer;
        public byte[] ReceiveBuffer;

        private TransportProtocol transportProtocol = new TransportProtocol();

        public TransportClient()
        {
            TcpSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TcpSocketClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            TcpSocketClient.ReceiveBufferSize = RecievedBufferSize;
            TcpSocketClient.SendBufferSize = SendBufferSize;
            ReceiveBuffer = new byte[RecievedBufferSize];
            SendBuffer = new byte[SendBufferSize];
        }

        ~TransportClient()
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
        public void DisconnectToServer()
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
            while (transportProtocol.RecievePackageCount > 0)
            {
                Package package = transportProtocol.GetLastPackage();

                if (package.Shutdown)
                {
                    OnServerShutdown?.Invoke();
                    OnDebugInfo?.Invoke($"Сервер был выключен {LocalAdressClient}");
                    DisconnectToServer();
                    return;
                }
                else if (package.AuthAndGetRSAKey)
                {
                    OnDebugInfo?.Invoke($"Авторизация RSA прошла успешна, {Encoding.UTF8.GetString(package.Body)}");
                    OnAuthorized?.Invoke();
                }
                else
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
                        byte[] data = new byte[received];
                        Array.Copy(ReceiveBuffer, data, received);
                        if (received != 0)
                            transportProtocol.CreatePackage(data);

                        ProcessInputData();

                        TcpSocketClient.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
                    }
                }
                catch (Exception ex) { OnExceptionInfo?.Invoke(ex); }
            }
        }

        private void SendData(Package package)
        {
            if (IsConnected)
            {
                IPEndPoint IPEndPoint = TcpSocketClient.RemoteEndPoint as IPEndPoint;
                if(IPEndPoint != null)
                {
                    package.IPConnection = Package.ConvertFromIpAddressToInteger(IPEndPoint.Address.ToString());
                    byte[] encodedPackage = transportProtocol.CreateBinaryData(package);
                    Array.Copy(encodedPackage, SendBuffer, encodedPackage.Length);
                    TcpSocketClient.BeginSend(SendBuffer, 0, encodedPackage.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
                }
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
            DisconnectToServer();
        }
    }
}
