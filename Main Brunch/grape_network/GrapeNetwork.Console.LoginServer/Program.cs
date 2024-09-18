using Grape;
using GrapeNetwork.Console.Common;
using GrapeNetwork.Server.BuilderServer;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Core.Protocol;
using Grpc.Net.Client;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace GrapeNetwork.Console.LoginServer
{
    class Program
    {
        static Server.Core.Server loginServer = new Server.Core.Server();
        private static void Main()
        {
            ConfigServer configServer = new ConfigServer() 
            {       
                ApplicationProtocol = new Protocol.LoginProtocol.LoginProtocol(),
                Services =  new System.Collections.Generic.List<Service>()
                {
                    new Server.Login.Service.AuthenticationService("AuthenticationService"),
                    new Server.Login.Service.RegistrationService("RegistrationService"),
                    new Server.Login.Service.LobbyService("LobbyService"),
                },
                ConfigServices = new System.Collections.Generic.List<ConfigService>()
                {
                    new ConfigService(),
                    new ConfigService(),
                    new ConfigService()
                },
                NameServer = "LoginServer",
                PortServer = 2200,
                IPAdressServer = IPAddress.Parse("192.168.1.100"),

                ConfigCommunicationServices = new System.Collections.Generic.List<ConfigCommunicationService>()
                {
                    new ConfigCommunicationService(IPAddress.Parse("192.168.1.100"), 3200),
                    new ConfigCommunicationService(IPAddress.Parse("192.168.1.100"), 3201),
                    new ConfigCommunicationService(IPAddress.Parse("192.168.1.100"), 3202),
                },
            };
            loginServer = BuilderServer.CreateServer(configServer);
            ConsoleManager.WriteLine("Запуск TL Login Server");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);
            loginServer.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            loginServer.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            loginServer.Run();
            ConsoleManager.Debug("Сервер запущен");

            ConsoleManager.ReadKey();
            loginServer.Stop();
            ConsoleManager.ReadKey();
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            loginServer.Stop();
        }
    }
}
