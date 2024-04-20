using GrapeNetwork.Core;
using GrapeNetwork.Core.Server;
using GrapeNetwork.Packages;
using GrapeNetwork.Server.GameServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core
{
    public class BaseServer
    {
        protected TransportServer transportServer;
        protected bool IsLog = true;
        protected int ServerID = 0;
        protected string NameServer;
        protected List<CommandProcessing> commandRegistry;
        protected TimerCallback timerCallback;
        protected Timer timer;
        protected event Action<Connection> OnConnectedClient;
        protected event Action<Connection> OnDisconnectedClient;
        protected event Action<Connection, Package> OnRecieveDataClient;
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnDebugInfo;
        public event Action TickServer;
        //public event Action<CommandProcessing> OnCommandProcessingPerfomed;
        //public event Action<CommandProcessing> OnCommandProcessingComplete;
        protected Queue<CommandProcessing> queueSendCommandProcessing = new Queue<CommandProcessing>(); 
        protected List<ClientState> clientStates = new List<ClientState>();
        protected List<BaseService> services = new List<BaseService> ();

        public virtual void Run()
        {
            transportServer = new TransportServer(2200, IPAddress.Parse("192.168.1.100"));
            transportServer.Start();

            transportServer.OnConnectedClient += ConnectedClient;
            transportServer.OnDisconnectedClient += DisconnectedClient;
            transportServer.OnRecieveDataClient += RecieveDataClient;
            if(IsLog)
            {
                transportServer.OnExceptionInfo += ExceptionInfo;
                transportServer.OnDebugInfo += DebugInfo;
            }

            foreach(BaseService service in services)
            {
                service.OnSendCommandProcessing += AddSendCommandProcessingToQueue;
                service.Init(this);
            }

            timerCallback = new TimerCallback(Tick);
            timer = new Timer(timerCallback, new object(), 0, 50);
        }
        protected virtual void Tick(object nullObj)
        {
            TickServer?.Invoke();
        }
        protected virtual void AddSendCommandProcessingToQueue(CommandProcessing commandProcessing)
        {
            queueSendCommandProcessing.Enqueue(commandProcessing);
        }
        public List<CommandProcessing> GetCommandRegistry()
        {
            return commandRegistry;
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
        public void ExceptionInfo(Exception exception) { OnExceptionInfo?.Invoke(exception);  }
        public void DebugInfo(string message) { OnDebugInfo?.Invoke(message); }
    }
}
