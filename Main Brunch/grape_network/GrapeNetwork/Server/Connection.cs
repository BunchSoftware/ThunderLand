using GrapeNetwork.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Core.Server
{
    public class Connection
    {
        public uint IDConnection;
        public Socket WorkSocket;
        public string RemoteAdressClient => WorkSocket.RemoteEndPoint.ToString();
        public byte[] SendBuffer;
        public byte[] ReceiveBuffer;

        public ConnectionState ConnectionState = ConnectionState.Disconnected;

        public bool IsConnected
        {
            get
            {
                try
                {
                    if (WorkSocket.Connected)
                    {
                        if (WorkSocket.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (WorkSocket.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                OnDisconnect?.Invoke();
                                ConnectionState = ConnectionState.Disconnected;
                                return false;
                            }
                            else
                            {
                                ConnectionState = ConnectionState.Connected;
                                return true;
                            }
                        }
                        ConnectionState = ConnectionState.Connected;
                        return true;
                    }
                    else
                    {
                        OnDisconnect?.Invoke();
                        ConnectionState = ConnectionState.Disconnected;
                        return false;
                    }
                }
                catch (Exception)
                {
                    ConnectionState = ConnectionState.Disconnected;
                    return false;
                }
            }
        }

        public Action OnDisconnect;

         public TransportProtocol transportProtocol = new TransportProtocol();

        public Connection(Socket socketToClient, int SendBufferSize, int RecievedBufferSize)
        {
            WorkSocket = socketToClient;
            WorkSocket.SendBufferSize = SendBufferSize;
            WorkSocket.ReceiveBufferSize = RecievedBufferSize;
            SendBuffer = new byte[SendBufferSize];
            ReceiveBuffer = new byte[RecievedBufferSize];
            ConnectionState = ConnectionState.Connected;
        }

        public void Close()
        {
            WorkSocket.Dispose();
        }
    }
}
