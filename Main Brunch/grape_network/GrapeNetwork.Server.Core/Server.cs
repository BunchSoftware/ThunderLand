using GrapeNetwork.Core.Server;
using GrapeNetwork.Server.Core.Protocol;
using GrapeNetwork.Server.Core.Grpc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using GrapeNetwork.Core;

namespace GrapeNetwork.Server.Core
{
    public class Server
    {
        // Настройки сервера
        private TransportServer transportServer;
        private IPAddress IPAdressServer;
        private int PortServer;
        private bool IsLog = true;
        private int ServerID = 0;
        private string NameServer;
        private int deltaTimeServer = 50;

        // Настройка обработки команд
        private List<ApplicationCommand> applicationCommandRegistry;
        private TimerCallback timerCallback;
        private Timer timer;

        // События от клиентов
        private event Action<Connection> OnConnectedClient;
        private event Action<Connection> OnDisconnectedClient;
        private event Action<Connection, Package> OnRecieveDataClient;
        // События от логов
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnDebugInfo;

        public event Action TickServer;

        // Данные
        protected Queue<ApplicationCommand> queueSendApplicationCommand = new Queue<ApplicationCommand>(); 
        protected List<ClientState> clientStates = new List<ClientState>();
        protected List<Service> services = new List<Service> ();

        private ApplicationProtocol ApplicationProtocol;
        private GrpcServer GrpcServer;

        public virtual void Run()
        {
            if (IPAdressServer != null)
            {
                transportServer = new TransportServer(PortServer, IPAdressServer);
                GrpcServer.Run();
                GrpcServer.SendPackage(new Package(), NameServer);

                transportServer.OnConnectedClient += ConnectedClient;
                transportServer.OnDisconnectedClient += DisconnectedClient;
                transportServer.OnRecieveDataClient += RecieveDataClient;

                if (IsLog)
                {
                    transportServer.OnExceptionInfo += ExceptionInfo;
                    transportServer.OnDebugInfo += DebugInfo;
                }

                transportServer.Start();

                foreach (Service service in services)
                {
                    service.OnSendApplicationCommand += AddSendApplicationCommandToQueue;
                    service.Init(this);
                }

                timerCallback = new TimerCallback(Tick);
                timer = new Timer(timerCallback, new object(), 0, deltaTimeServer);

                transportServer.OnRecieveDataClient += (connection, package) =>
                {
                    ApplicationProtocol.CreatePackage(package);
                    ApplicationCommand commandProcessing = ApplicationProtocol.GetLastCommandProcessing();
                    commandProcessing.Connection = connection;
                    if (commandProcessing != null)
                    {
                        for (int i = 0; i < services.Count; i++)
                        {
                            if (services[i].nameService == commandProcessing.NameService)
                            {
                                ClientState clientState = new ClientState(connection);
                                for (int j = 0; j < clientStates.Count; j++)
                                {
                                    if (clientStates[j].connection == connection)
                                        clientState = clientStates[j];
                                }
                                services[i].AddApplicationCommand(commandProcessing, clientState);
                            }
                        }
                    }
                };
                transportServer.OnConnectedClient += (connection) =>
                {
                    clientStates.Add(new ClientState(connection));
                    Package package = new Package();
                    package.AuthAndGetRSAKey = true;
                    package.Body = Encoding.UTF8.GetBytes("RSA Key");
                    transportServer.SendPackage(connection, package);
                };
                transportServer.OnDisconnectedClient += (connection) =>
                {
                    for (int i = 0; i < clientStates.Count; i++)
                    {
                        if (clientStates[i].connection == connection)
                            clientStates.Remove(clientStates[i]);
                    }
                };
            }
            else
            {
                throw new Exception();
            }
        }
        protected virtual void Tick(object nullObj)
        {
            TickServer?.Invoke();
            for (int i = 0; i < queueSendApplicationCommand.Count; i++)
            {
                ApplicationCommand applicationCommand = queueSendApplicationCommand.Dequeue();
                if (applicationCommand.RedirectToService == false)
                {
                    IPEndPoint IPEndPoint = (IPEndPoint)applicationCommand.Connection.WorkSocket.RemoteEndPoint;
                    Package package = new Package(IPEndPoint.Address, applicationCommand.GroupCommand, applicationCommand.Command);
                    if (applicationCommand.Connection != null)
                        transportServer.SendPackage(applicationCommand.Connection, package);
                }
                else
                {
                    for (int g = 0; g < services.Count; g++)
                    {
                        if (services[g].nameService == applicationCommand.NameService)
                        {
                            ClientState clientState = new ClientState(applicationCommand.Connection);
                            for (int j = 0; j < clientStates.Count; j++)
                            {
                                if (clientStates[j].connection == applicationCommand.Connection)
                                    clientState = clientStates[j];
                            }
                            services[i].AddApplicationCommand(applicationCommand, clientState);
                        }
                        else
                        {

                        }
                    }
                }
            }
        }
        private void AddSendApplicationCommandToQueue(ApplicationCommand applicationCommand)
        {
            queueSendApplicationCommand.Enqueue(applicationCommand);
        }
        public List<ApplicationCommand> GetApplicationCommandRegistry()
        {
            return applicationCommandRegistry;
        }
        public void ReadConfig(Configuration.ConfigServer configServer)
        {
            services = configServer.Services;
            for (int i = 0; i < services.Count; i++)
            {
                services[i].ReadConfig(configServer.ConfigServices[i]);
            }
            ApplicationProtocol = configServer.ApplicationProtocol;
            NameServer = configServer.NameServer;
            PortServer = configServer.PortServer;
            IPAdressServer = configServer.IPAdressServer;
            GrpcServer = configServer.GrpcServer;
            OnDebugInfo?.Invoke("Конфигурации сервера прочитаны и сервер настроен");
        }
        public void Stop()
        {
            GrpcServer.Stop();
            transportServer.Stop();
        }
        private void ConnectedClient(Connection connection) {  OnConnectedClient?.Invoke(connection); }
        private void DisconnectedClient(Connection connection) { OnDisconnectedClient?.Invoke(connection);  }
        private void RecieveDataClient(Connection connection, Package package) { OnRecieveDataClient?.Invoke(connection, package); }
        public void ExceptionInfo(Exception exception) { OnExceptionInfo?.Invoke(exception);  }
        public void DebugInfo(string message) { OnDebugInfo?.Invoke(message); }
    }
}
