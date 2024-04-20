using GrapeNetwork.AccessLevel.Common;
using GrapeNetwork.Core.Client;
using GrapeNetwork.Packages;
using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
using GrapeNetwork.Protocol.LoginProtocol.Command.Registration;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Client.Core
{
    public class GameClient
    {
        protected List<CommandProcessing> commandRegistry;
        protected TransportClient transportClient = new TransportClient();
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnErrorInfo;
        public event Action<string> OnDebugInfo;

        public void ConnectToServer()
        {
            transportClient.ConnectToServer(2200, IPAddress.Parse("192.168.1.100"));
            transportClient.OnDebugInfo += DebugInfo;
            transportClient.OnExceptionInfo += ExceptionInfo;

            transportClient.OnRecieveDataEvent += (package) =>
            {
                switch (package.Command)
                {
                    case 2:
                        {
                            DebugInfo("Авторизация на сервере прошла успешна, просим напрвиться в лобби");
                            DebugInfo("Вход в лобби");
                            DebugInfo("Вход в лобби произошел успешно !");
                            break;
                        }
                    case 3:
                        {
                            ErrorInfo("Логин или пароль были не верны, проверьте правильность вводимых данных");
                            break;
                        }
                    case 5:
                        {
                            DebugInfo("Регистрация прошла успешна ! Желаем хорошой игры !");
                            break;
                        }
                    case 6:
                        {
                            ErrorInfo("Такой акаунт уже зарегистрирован");
                            break;
                        }
                    case 8:
                        {
                            ErrorInfo("Запрос игровым сервером был отклонен, попробуйте еще раз !");
                            break;
                        }
                    case 9:
                        {
                            DebugInfo("Игровой сервер принял вызов !");
                            DebugInfo("Вход в игровой мир");
                            DebugInfo("Вход в игровой мир произошел успешно !");
                            break;
                        }
                    default:
                        break;
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
                IPConnection = Package.ConvertFromIpAddressToInteger("192.168.1.100"),
                IDConnection = 1,
                GroupCommand = 1,
                Command = 1,
                Body = memoryStream.ToArray()
            };
            transportClient.SendPackage(package, true);
        }
        public void EnterLobby()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write("192.168.1.100");
            writer.Write("2201");
            Package package = new Package()
            {
                IPConnection = Package.ConvertFromIpAddressToInteger("192.168.1.100"),
                IDConnection = 1,
                GroupCommand = 1,
                ReconnectionOtherServer = true,
                Command = 7,
                Body = memoryStream.ToArray()
            };
            transportClient.SendPackage(package, true);
        }
        public void Registration(string login, string password)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(login);
            writer.Write(password);
            Package package = new Package()
            {
                IPConnection = Package.ConvertFromIpAddressToInteger("192.168.1.100"),
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
        }

        public void ExceptionInfo(Exception exception) { OnExceptionInfo?.Invoke(exception); }
        public void DebugInfo(string message) { OnDebugInfo?.Invoke(message); }
        public void ErrorInfo(string message) { OnErrorInfo?.Invoke(message); }
    }
}
