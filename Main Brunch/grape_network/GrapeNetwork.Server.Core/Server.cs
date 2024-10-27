using GrapeNetwork.Core.Server;
using GrapeNetwork.Server.Core.Protocol;
using GrapeNetwork.Server.Core.Grpc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using GrapeNetwork.Core;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Configurator.Interfaces;

namespace GrapeNetwork.Server.Core
{
    public class Server
    {
        // Настройки сервера
        private TransportServer transportServer;
        private IPAddress IPAddressServer;
        private int PortServer;
        private bool IsLog = true;
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

        protected List<Service> InternalServices = new List<Service> ();
        protected List<Service> ExternalServices = new List<Service>();

        private ApplicationProtocol ApplicationProtocol;
        private GrpcServer GrpcServer;

        public virtual void Run()
        {
            if (IPAddressServer != null)
            {
                transportServer = new TransportServer(PortServer, IPAddressServer);

                if (GrpcServer != null)
                    GrpcServer.Run();

                transportServer.OnConnectedClient += ConnectedClient;
                transportServer.OnDisconnectedClient += DisconnectedClient;
                transportServer.OnRecieveDataClient += RecieveDataClient;

                if (IsLog)
                {
                    transportServer.OnExceptionInfo += ExceptionInfo;
                    transportServer.OnDebugInfo += DebugInfo;
                }

                transportServer.Start();

                foreach (Service service in InternalServices)
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
                        for (int i = 0; i < InternalServices.Count; i++)
                        {
                            if (InternalServices[i].nameService == commandProcessing.NameService)
                            {
                                ClientState clientState = new ClientState(connection);
                                for (int j = 0; j < clientStates.Count; j++)
                                {
                                    if (clientStates[j].connection == connection)
                                        clientState = clientStates[j];
                                }
                                InternalServices[i].AddApplicationCommand(commandProcessing, clientState);
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
        protected void Tick(object nullObj)
        {
            TickServer?.Invoke();
            for (int i = 0; i < queueSendApplicationCommand.Count; i++)
            {
                ApplicationCommand applicationCommand = queueSendApplicationCommand.Dequeue();
                for (int g = 0; g < InternalServices.Count; g++)
                {
                    if (InternalServices[g].nameService == applicationCommand.NameService)
                    {
                        ClientState clientState = new ClientState(applicationCommand.Connection);
                        for (int j = 0; j < clientStates.Count; j++)
                        {
                            if (clientStates[j].connection == applicationCommand.Connection)
                                clientState = clientStates[j];
                        }
                        InternalServices[i].AddApplicationCommand(applicationCommand, clientState);
                    }
                    else if(InternalServices[g].nameService == null || InternalServices[g].nameService == "")
                    {
                        IPEndPoint IPEndPoint = (IPEndPoint)applicationCommand.Connection.WorkSocket.RemoteEndPoint;
                        Package package = new Package(IPEndPoint.Address, applicationCommand.GroupCommand, applicationCommand.Command);
                        if (applicationCommand.Connection != null)
                            transportServer.SendPackage(applicationCommand.Connection, package);
                    }
                    else
                    {
                        if (ExternalServices != null)
                        {
                            for (int k = 0; k < ExternalServices.Count; k++)
                            {
                                IPEndPoint IPEndPoint = (IPEndPoint)applicationCommand.Connection.WorkSocket.RemoteEndPoint;
                                Package package = new Package(IPEndPoint.Address, applicationCommand.GroupCommand, applicationCommand.Command);
                                GrpcServer.SendPackage(package, applicationCommand.NameService);
                            }
                        }
                        else
                            throw new Exception($"Ошибка в написании имени сервиса, ошибка с сервисом {applicationCommand.NameService}");
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
        public void ReadConfig(ConfigServer config)
        {
            if (config != null)
            {
                ApplicationProtocol = config.GetSection<ApplicationProtocol>("ApplicationProtocol");
                IPAddressServer = config.GetSection<IPAddress>("IPAddressServer");
                PortServer = config.GetSection<int>("PortServer");

                if (ApplicationProtocol == null)
                    throw new NullReferenceException();
                if(IPAddressServer == null)
                    throw new NullReferenceException();

                List<ConfigService> InternalServicesConfig = config.GetSection<List<ConfigService>> ("InternalServicesConfig");
                if (InternalServicesConfig != null && InternalServicesConfig.Count != 0)
                {
                    List<IRepository<Service>> RepositoryServicesList = config.GetSection<List<IRepository<Service>>>("RepositoryServices");
                    for (int i = 0; i < RepositoryServicesList.Count; i++)
                    {
                        List<Service> repositoryServices = (List<Service>)RepositoryServicesList[i].GetObjectList();
                        for (int j = 0; j < InternalServicesConfig.Count; j++)
                        {
                            for (int k = 0; k < repositoryServices.Count; k++)
                            {
                                if (InternalServicesConfig[j].GetSection<string>("NameService") == repositoryServices[k].nameService)
                                {
                                    Service service = repositoryServices[k];
                                    service.ReadConfig(InternalServicesConfig[j]);
                                    InternalServices.Add(repositoryServices[k]);
                                }
                            }
                        }
                    }
                }

                List<ConfigService> ExternalSericesConfig = config.GetSection<List<ConfigService>>("ExternalServicesConfig");
                if (ExternalSericesConfig != null && ExternalSericesConfig.Count != 0)
                {
                    List<IRepository<Service>> RepositoryServicesList = config.GetSection<List<IRepository<Service>>>("RepositoryServices");
                    for (int i = 0; i < RepositoryServicesList.Count; i++)
                    {
                        List<Service> repositoryServices = (List<Service>)RepositoryServicesList[i].GetObjectList();
                        for (int j = 0; j < ExternalSericesConfig.Count; j++)
                        {
                            for (int k = 0; k < repositoryServices.Count; k++)
                            {
                                if (ExternalSericesConfig[j].GetSection<string>("NameService") == repositoryServices[k].nameService)
                                {
                                    Service service = repositoryServices[k];
                                    service.ReadConfig(ExternalSericesConfig[j]);
                                    InternalServices.Add(repositoryServices[k]);
                                }
                            }
                        }
                    }
                }

                NameServer = config.GetSection<string>("NameServer");
                GrpcServer = config.GetSection<GrpcServer>("GrpcServer");
                IsLog = config.GetSection<bool>("IsLog");
            }
            else
                throw new NullReferenceException();

            OnDebugInfo?.Invoke("Конфигурации сервера прочитаны и сервер настроен");
        }
        public void Stop()
        {
            if(GrpcServer != null)
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
