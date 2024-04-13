using GrapeNetwork.Core.Server;
using GrapeNetwork.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core
{
    public class BaseServer
    {
        protected TransportServer transportServer;
        public bool isLog = true;
        public int ServerID = 0;
        public string NameServer = "";

        protected event Action<Connection> OnConnectedClient;
        protected event Action<Connection> OnDisconnectedClient;
        protected event Action<Connection, Package> OnRecieveDataClient;
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnDebugInfo;

        public virtual void Run()
        {
            transportServer = new TransportServer(2200, IPAddress.Parse("192.168.1.100"));
            transportServer.Start();

            transportServer.OnConnectedClient += ConnectedClient;
            transportServer.OnDisconnectedClient += DisconnectedClient;
            transportServer.OnRecieveDataClient += RecieveDataClient;
            if(isLog)
            {
                transportServer.OnExceptionInfo += ExceptionInfo;
                transportServer.OnDebugInfo += DebugInfo;
            }
        }
        public virtual string ReadConfig()
        {
            return "config";
        }
        public virtual void Stop()
        {
            transportServer.Stop();
        }
        protected void ConnectedClient(Connection connection) { OnConnectedClient?.Invoke(connection); }
        protected void DisconnectedClient(Connection connection) { OnDisconnectedClient?.Invoke(connection);  }
        protected void RecieveDataClient(Connection connection, Package package) { OnRecieveDataClient?.Invoke(connection, package); }
        protected void ExceptionInfo(Exception exception) { OnExceptionInfo?.Invoke(exception);  }
        protected void DebugInfo(string message) { OnDebugInfo?.Invoke(message); }
    }
}
