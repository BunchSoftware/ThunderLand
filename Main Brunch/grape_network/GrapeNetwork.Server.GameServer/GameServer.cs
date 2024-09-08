using GrapeNetwork.Packages;
using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
using GrapeNetwork.Protocol.LoginProtocol.Command.Registration;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Protocol.GameProtocol;
using System.Collections.Generic;
using System.Text;
using System.Net;
using GrapeNetwork.Core.Client;
using GrapeNetwork.Server.Core.Command;
using GrapeNetwork.Server.Core.CommonService;
using System.IO;
using GrapeNetwork.Server.Core.Protocol;

namespace GrapeNetwork.Server.GameServer
{
    public class GameServer : BaseServer
    {
        protected GameProtocol gameProtocol;
        protected TransportClient loginServerCommunication;

        public override void Run()
        {
            base.Run();
            transportServer.OnRecieveDataClient += (connection, package) =>
            {
                gameProtocol.CreatePackage(package);
                ApplicationCommand commandProcessing = gameProtocol.GetLastCommandProcessing();
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
                            services[i].AddCommandProcessing(commandProcessing, clientState);
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
                clientStates.RemoveAt((int)connection.IDConnection - 1);
            };
        }
        protected override void Tick(object nullObj)
        {
            base.Tick(nullObj);
            for (int i = 0; i < queueSendCommandProcessing.Count; i++)
            {
                ApplicationCommand commandProcessing = queueSendCommandProcessing.Dequeue();
                Package package = new Package()
                {
                    GroupCommand = commandProcessing.GroupCommand,
                    Command = commandProcessing.Command,
                };
                if (commandProcessing.Connection != null)
                    transportServer.SendPackage(commandProcessing.Connection, package);
            }
        }
        public override string ReadConfig()
        {
            commandRegistry = new List<ApplicationCommand>()
            {
                //new RequestConnectToLoginServer(1, 1, "AuthenticationService"),
                //new RequestRegistrationUser(1, 4, "RegistrationService"),
                //new RequestToGameServerForUserConnection(1, 7, "LobbyService"),
                //new ResponseSendClientToLobby(1,2, "AuthenticationService"),
                //new ResponseRejectedLobbyConnection(1,3, "AuthenticationService"),
                //new ResponseUserRegistrationConfirmation(1,5, "RegistrationService"),
                //new ResponseRejectedRegistrationUser(1,6, "RegistrationService"),
                //new ResponseRejectedUserConnectionToGameServer(1,8, "LobbyService"),
                //new ResponseConnectingUserToGameServer(1,9, "LobbyService"),
                //new RequestConnectToServer(4,1,"ServerCommunicationService"),
                //new ResponseConnectToServer(4,2, "ServerCommunicationService"),
                //new ResponseRejectedConnectToServer(4,3,"ServerCommunicationService"),
            };
            services = new List<Service>
            {
                //new ServerCommunicationService("ServerCommunicationService")
            };
            foreach (Service baseService in services)
            {
                baseService.ReadConfig();
            }
            gameProtocol = new GameProtocol(commandRegistry);
            NameServer = "GameServer";
            PortServer = 2201;
            IPAdressServer = IPAddress.Parse("192.168.56.1");           

            return "config";
        }
        public override void Stop()
        {
            base.Stop();
        }
    }
}
