using GrapeNetwork.Packages;
using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
using GrapeNetwork.Protocol.LoginProtocol.Command.Registration;
using GrapeNetwork.Protocol.LoginProtocol;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.LoginServer.Service;
using GrapeNetwork.Protocol.GameProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using GrapeNetwork.Core.Client;
using GrapeNetwork.Server.Core.Command;
using GrapeNetwork.Server.Core.CommonService;
using System.IO;

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
                CommandProcessing commandProcessing = gameProtocol.GetLastCommandProcessing();
                commandProcessing.Connection = connection;
                if (commandProcessing != null)
                {
                    for (int i = 0; i < services.Count; i++)
                    {
                        if (services[i].nameService == commandProcessing.NameService)
                        {
                            services[i].AddCommandProcessing(commandProcessing, new ClientState(connection));
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
                CommandProcessing commandProcessing = queueSendCommandProcessing.Dequeue();
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
            commandRegistry = new List<CommandProcessing>()
            {
                new RequestConnectToLoginServer(1, 1, "AuthenticationService"),
                new RequestRegistrationUser(1, 4, "RegistrationService"),
                new RequestToGameServerForUserConnection(1, 7, "LobbyService"),
                new ResponseSendClientToLobby(1,2, "AuthenticationService"),
                new ResponseRejectedLobbyConnection(1,3, "AuthenticationService"),
                new ResponseUserRegistrationConfirmation(1,5, "RegistrationService"),
                new ResponseRejectedRegistrationUser(1,6, "RegistrationService"),
                new ResponseRejectedUserConnectionToGameServer(1,8, "LobbyService"),
                new ResponseConnectingUserToGameServer(1,9, "LobbyService"),
                new RequestConnectToServer(4,1,"ServerCommunicationService"),
                new ResponseConnectToServer(4,2, "ServerCommunicationService"),
                new ResponseRejectedConnectToServer(4,3,"ServerCommunicationService"),
            };
            services = new List<BaseService>
            {
                new ServerCommunicationService("ServerCommunicationService")
            };
            foreach (BaseService baseService in services)
            {
                baseService.ReadConfig();
            }
            gameProtocol = new GameProtocol(commandRegistry);
            NameServer = "GameServer";
            PortServer = 2201;
            IPAdressServer = IPAddress.Parse("192.168.1.100");


            CommandProcessing commandProcessing = new CommandProcessing(4, 1, "ServerCommunicationService");
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(2201);
            writer.Write("192.168.1.100");
            writer.Write(2200);
            writer.Write("192.168.1.100");
            commandProcessing.CommandData = memoryStream.ToArray();
            if (commandProcessing != null)
            {
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i].nameService == commandProcessing.NameService)
                    {
                        services[i].AddCommandProcessing(commandProcessing, null);
                    }
                }
            }

            return "config";
        }
        public override void Stop()
        {
            base.Stop();
        }
    }
}
