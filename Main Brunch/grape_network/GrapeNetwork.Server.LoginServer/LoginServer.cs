using GrapeNetwork.Packages;
using GrapeNetwork.Server.Core;
using System.Collections.Generic;
using System.Text;
using GrapeNetwork.Protocol.LoginProtocol;
using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
using GrapeNetwork.Protocol.LoginProtocol.Command.Registration;
using System.Net;
using GrapeNetwork.Server.LoginServer.Service;

namespace GrapeNetwork.Server.LoginServer
{
    public class LoginServer : BaseServer
    {
        protected LoginProtocol loginProtocol;

        public override void Run()
        {
            base.Run();
            transportServer.OnRecieveDataClient += (connection, package) =>
            {
                loginProtocol.CreatePackage(package);
                CommandProcessing commandProcessing = loginProtocol.GetLastCommandProcessing();
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
                if(commandProcessing.Connection != null)
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
            };
            services = new List<BaseService> 
            {
                new AuthenticationService("AuthenticationService"),
                new RegistrationService("RegistrationService"),
                new LobbyService("LobbyService"),
            };
            foreach (BaseService baseService in services)
            {
                baseService.ReadConfig();
            }
            loginProtocol = new LoginProtocol(commandRegistry);
            NameServer = "LoginServer";
            PortServer = 2200;
            IPAdressServer = IPAddress.Parse("192.168.56.1");

            return "config";
        }
        public override void Stop()
        {
            base.Stop();
        }
    }
}
