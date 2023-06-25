using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using UnityEditor.PackageManager;
using UnityEngine;

namespace ClientNet
{
    public class Client : ICommandClient, IDisposable
    {
        /// <summary>
        /// Индефикатор клиент на сервере
        /// </summary>
        public int Id;

        private Socket _TcpSocketClient;
        /// <summary>
        /// Socket - сетевой компонент для общения с сервером
        /// </summary>
        public Socket TcpSocketClient
        {
            set
            {
                _TcpSocketClient = value;
                if (TcpSocketClient.Connected)
                    _ConnectionState = ConnectionState.Connected;
            }
            get => _TcpSocketClient;
        }

        private ConnectionState _ConnectionState = ConnectionState.Disconnected;
        /// <summary>
        /// Состояние подключения клиента к серверу
        /// </summary>
        public ConnectionState ConnectionState { get => _ConnectionState; }

        /// <summary>
        /// Подключен ли клиент к серверу
        /// </summary>
        public bool isConnected
        {
            get
            {
                if (ConnectionState == ConnectionState.Connected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Событие при подключении к серверу
        /// </summary>
        public delegate void ConnectEvent();
        public event ConnectEvent OnConnectEvent;

        /// <summary>
        /// Событие при отключении от сервера
        /// </summary>
        public delegate void DisconnectEvent();
        public event DisconnectEvent OnDisconnectEvent;

        /// <summary>
        /// Событие отправки пакетов данных
        /// </summary>
        public delegate void SendEvent();
        public event SendEvent OnSendEvent;

        /// <summary>
        /// Событие получения пакетов данных
        /// </summary>
        /// <param name="package">Пакеты данных,  получаемые от сервера</param>
        public delegate void RecieveEvent(object package);
        public event RecieveEvent OnRecieveEvent;

        
        private int CountOfNegatives = 5; // Количество попыток подключений

        private byte[] BufferReceive = new byte[1024];
        private byte[] BufferSend = new byte[1024];

        public Client()
        {
            _TcpSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        ~Client()
        {
            Dispose();
        }

        /// <summary>
        /// Устанавливает подключение с сервером
        /// </summary>
        /// <param name="IPAddressServer">IpAddress клиента</param>
        /// <param name="portServer">Номер порта клиента</param>
        /// <param name="passwordServer">Пароль при подключении к серверу</param>
        public void ConnectToServer(IPAddress IPAddressServer, int portServer, string passwordServer)
        {
            _ConnectionState = ConnectionState.Connecting;
            for (int i = 0; i < CountOfNegatives & !TcpSocketClient.Connected; i++)
            {
                try
                {
                    TcpSocketClient.Connect(IPAddressServer, portServer);
                    _ConnectionState = ConnectionState.Connected;
                    OnConnectEvent?.Invoke();
                    TcpSocketClient.BeginReceive(BufferReceive, 0, BufferReceive.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), TcpSocketClient);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// Закрывает подключение к серверу
        /// </summary>
        public void DisconnectFromServer()
        {
            if (TcpSocketClient.Connected)
            {
                try
                {
                    Package package = new Package();
                    //package.RequestsView = RequestsView.Exit;
                    //SendData(jsonSerialize.Serialize(package));

                    _ConnectionState = ConnectionState.Disconnected;
                    OnDisconnectEvent?.Invoke();
                    TcpSocketClient.Disconnect(false);
                    TcpSocketClient.Close();
                    TcpSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                catch (Exception)
                {

                }
            }
        }
        /// <summary>
        /// Обратная связь с сервером при получении пакетов данных
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            if (TcpSocketClient.Connected)
            {
                int received = TcpSocketClient.EndReceive(asyncResult);
                byte[] dataBuffer = new byte[received];
                Array.Copy(BufferReceive, dataBuffer, received);
                string data = Encoding.UTF8.GetString(dataBuffer);
                //Package package = JsonConvert.DeserializeObject(data); 

                //if (package.RequestsView == RequestsView.Exit)
                //{
                //    TcpSocketClient.Close();
                //    return;
                //}
                //else if (package.RequestsView == RequestsView.Get)
                //{
                //    OnRecieveEvent?.Invoke(package);
                //}

                TcpSocketClient.BeginReceive(BufferReceive, 0, BufferReceive.Length, SocketFlags.None, ReceiveCallback, null);
            }
        }
        /// <summary>
        /// Отправка пакетов данных на сервер
        /// По умолчанию: оповещает клиента об отправке пакета данных
        /// </summary>
        /// <param name="package"></param>
        public void SendData(Package package)
        {
            if (TcpSocketClient.Connected)
            {
                BufferSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(package));
                TcpSocketClient.BeginSend(BufferSend, 0, BufferSend.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
                OnSendEvent?.Invoke();
            }
        }
        /// <summary>
        /// Отправка пакетов данных на сервер
        /// </summary>
        /// <param name="package">Пакет данных для отправки</param>
        /// <param name="isCallSendEvent">Оповещать ли клиента об отправке пакета данных</param>
        public void SendData(Package package, bool isCallSendEvent)
        {
            if (TcpSocketClient.Connected)
            {
                BufferSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(package));
                TcpSocketClient.BeginSend(BufferSend, 0, BufferSend.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);

                if (isCallSendEvent)
                    OnSendEvent?.Invoke();
            }
        }

        /// <summary>
        /// Обратная связь с сервером при отправке пакетов данных
        /// </summary>
        /// <param name="asyncResult"></param>
        private void SendCallback(IAsyncResult asyncResult)
        {
            TcpSocketClient.EndSend(asyncResult);
        }


        public void Dispose()
        {
            _ConnectionState = ConnectionState.Disconnected;
            _TcpSocketClient.Dispose();
        }
    }
}
