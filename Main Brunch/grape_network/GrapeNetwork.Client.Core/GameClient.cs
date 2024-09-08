using GrapeNetwork.Core.Client;
using GrapeNetwork.Server.Core.Protocol;
using GrapeNetwork.Packages;
using GrapeNetwork.Protocol.LoginProtocol;
using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
using GrapeNetwork.Protocol.LoginProtocol.Command.Registration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GrapeNetwork.Protocol.GameProtocol.Command.Account.Get;
using GrapeNetwork.Protocol.GameProtocol;

namespace GrapeNetwork.Client.Core
{
    public class GameClient
    {
        protected List<ApplicationCommand> commandRegistry;
        protected TransportClient transportClient = new TransportClient();

        protected LoginProtocol loginProtocol;
        protected GameProtocol gameProtocol;

        protected string IPAdressLoginServer = "192.168.1.100";
        protected int PortLoginServer = 2200;

        protected string IPAdressGameServer = "192.168.1.100";
        protected int PortGameServer = 2201;

        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnErrorInfo;
        public event Action<string> OnDebugInfo;

        public event Action OnConnectEvent;
        public event Action OnDisconnectEvent;
        public event Action OnServerShutdown;
        public event Action OnAuthorizedRSA;

        protected string IPAdressClient = "192.168.1.100";

        private bool isAuth = false;
        public bool IsAuth {  get { return isAuth; } }

        private bool isLobby = false;
        public bool IsLobby { get { return isLobby; } }

        private bool isGame = false;
        public bool IsGame { get { return isGame; } }

        public GameClient() 
        { 
            transportClient.OnServerShutdown += ServerShutdown;
            transportClient.OnConnectEvent += ConnectEvent;
            transportClient.OnDisconnectEvent += DisconnectEvent;
            transportClient.OnAuthorizedRSA += AuthorizedRSA;

            transportClient.OnExceptionInfo += ExceptionInfo;
            transportClient.OnDebugInfo += DebugInfo;
        }

        public void ConnectToLoginServer()
        {
            transportClient.ConnectToServer(PortLoginServer, IPAddress.Parse(IPAdressLoginServer));

            transportClient.OnRecieveDataEvent += (package) =>
            {
                loginProtocol.CreatePackage(package);
                ApplicationCommand commandProcessing = loginProtocol.GetLastCommandProcessing();

                Action<string> DebugInfo = (message) => { this.DebugInfo(message); };
                Action<string> ErrorInfo = (message) => { this.ErrorInfo(message); };

                if (commandProcessing != null)
                {
                    switch (commandProcessing.GroupCommand)
                    {
                        case 1:
                            {
                                switch (package.Command)
                                {
                                    case 2:
                                        {
                                            ResponseSendClientToLobby command = new ResponseSendClientToLobby(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                                            command.CommandData = commandProcessing.CommandData;
                                            command.Connection = commandProcessing.Connection;
                                            command.Execute(new object[] { DebugInfo });

                                            // Исправить. Должны получать от сервера данные
                                            isAuth = true;
                                            isLobby = true;

                                            break;
                                        }
                                    case 3:
                                        {
                                            ResponseRejectedLobbyConnection command = new ResponseRejectedLobbyConnection(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                                            command.CommandData = commandProcessing.CommandData;
                                            command.Connection = commandProcessing.Connection;
                                            command.Execute(new object[] { ErrorInfo });

                                            break;
                                        }
                                    case 5:
                                        {
                                            ResponseUserRegistrationConfirmation command = new ResponseUserRegistrationConfirmation(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                                            command.CommandData = commandProcessing.CommandData;
                                            command.Connection = commandProcessing.Connection;
                                            command.Execute(new object[] { DebugInfo });

                                            break;
                                        }
                                    case 6:
                                        {
                                            ResponseRejectedRegistrationUser command = new ResponseRejectedRegistrationUser(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                                            command.CommandData = commandProcessing.CommandData;
                                            command.Connection = commandProcessing.Connection;
                                            command.Execute(new object[] { ErrorInfo });

                                            break;
                                        }
                                    case 8:
                                        {
                                            ResponseRejectedUserConnectionToGameServer command = new ResponseRejectedUserConnectionToGameServer(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                                            command.CommandData = commandProcessing.CommandData;
                                            command.Connection = commandProcessing.Connection;
                                            command.Execute(new object[] { ErrorInfo });

                                            break;
                                        }
                                    case 9:
                                        {
                                            ResponseConnectingUserToGameServer command = new ResponseConnectingUserToGameServer(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                                            command.CommandData = commandProcessing.CommandData;
                                            command.Connection = commandProcessing.Connection;
                                            command.Execute(new object[] { DebugInfo });

                                            // Исправить. Должны получать от сервера данные
                                            isLobby = false;
                                            isGame = true;

                                            break;
                                        }
                                }
                                break;
                            }
                    }
                }
            };
        }

        public void Authetication(string login, string password)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(login);
            writer.Write(password);
            Package package = new Package()
            {
                IPConnection = Package.ConvertFromIpAddressToInteger(IPAdressClient),
                IDConnection = 1,
                GroupCommand = 1,
                Command = 1,
                Body = memoryStream.ToArray()
            };
            transportClient.SendPackage(package, true);
        }
        public void ConnectToGameServer()
        {
            Package packageLogin = new Package()
            {
                IPConnection = Package.ConvertFromIpAddressToInteger(IPAdressClient),
                IDConnection = 1,
                GroupCommand = 1,
                ReconnectionOtherServer = true,
                Command = 7,
            };
            transportClient.SendPackage(packageLogin, true);

            transportClient.DisconnectToServer();
            transportClient.ConnectToServer(PortGameServer, IPAddress.Parse(IPAdressGameServer));

            transportClient.OnRecieveDataEvent += (package) =>
            {
                gameProtocol.CreatePackage(package);
                ApplicationCommand commandProcessing = gameProtocol.GetLastCommandProcessing();

                Action<string> DebugInfo = (message) => { this.DebugInfo(message); };
                Action<string> ErrorInfo = (message) => { this.ErrorInfo(message); };
                Action<Package> SendPackage = (package) => { transportClient.SendPackage(package, true); };
                string IPAdressClient = this.IPAdressClient;

                if (commandProcessing != null)
                {
                    switch (commandProcessing.GroupCommand)
                    {
                        case 2:
                            {
                                switch (commandProcessing.Command)
                                {
                                    case 2:
                                        {
                                            ResponseGetDataAccount command = new ResponseGetDataAccount(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                                            command.CommandData = commandProcessing.CommandData;
                                            command.Connection = commandProcessing.Connection;
                                            command.Execute(new object[] { DebugInfo, SendPackage, IPAdressClient });
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                }
            };

            Package packageGame = new Package()
            {
                IPConnection = Package.ConvertFromIpAddressToInteger(IPAdressClient),
                IDConnection = 1,
                GroupCommand = 2,
                Command = 1,
            };
            transportClient.SendPackage(packageGame, true);
        }
        public void Registration(string login, string password)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(login);
            writer.Write(password);
            Package package = new Package()
            {
                IPConnection = Package.ConvertFromIpAddressToInteger(IPAdressClient),
                IDConnection = 1,
                GroupCommand = 1,
                Command = 4,
                Body = memoryStream.ToArray()
            };
            transportClient.SendPackage(package, true);
        }

        public void DisconnectFromServer()
        {
            transportClient.DisconnectToServer();
        }

        public void ReadConfig()
        {
            loginProtocol = new LoginProtocol();
            gameProtocol = new GameProtocol();
        }

        public void ExceptionInfo(Exception exception) { OnExceptionInfo?.Invoke(exception); }
        public void DebugInfo(string message) { OnDebugInfo?.Invoke(message); }
        public void ErrorInfo(string message) { OnErrorInfo?.Invoke(message); }
        protected void ServerShutdown() { OnServerShutdown?.Invoke();  }
        protected void DisconnectEvent() { OnDisconnectEvent?.Invoke();  }
        protected void AuthorizedRSA() { OnAuthorizedRSA?.Invoke(); }
        protected void ConnectEvent() { OnConnectEvent?.Invoke(); }
    }
}
