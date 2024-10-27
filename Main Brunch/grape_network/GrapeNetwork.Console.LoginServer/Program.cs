using Grape;
using GrapeNetwork.Configurator.Interfaces;
using GrapeNetwork.Console.Common;
using GrapeNetwork.Server.BuilderServer;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Core.Protocol;
using GrapeNetwork.Server.Database.Service;
using GrapeNetwork.Server.Game.Service;
using GrapeNetwork.Server.Login.Service;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
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
            ConsoleManager.WriteLine("Запуск TL Login Server");
            ConfigServer configServer = new ConfigServer();
            configServer.ChangeValueSection("ApplicationProtocol", new Protocol.LoginProtocol.LoginProtocol());

            ConfigService configAuthenticationService = new ConfigService();
            configAuthenticationService.ChangeValueSection("NameService", "AuthenticationService");

            ConfigService configRegistrationService = new ConfigService();
            configRegistrationService.ChangeValueSection("NameService", "RegistrationService");

            ConfigService configLobbyService = new ConfigService();
            configLobbyService.ChangeValueSection("NameService", "LobbyService");

            configServer.ChangeValueSection("InternalServicesConfig", new List<ConfigService>()
            {
                 configAuthenticationService,
                 configRegistrationService,
                 configLobbyService
            });
            configServer.ChangeValueSection("RepositoryServices", new List<IRepository<Service>>()
            {
                new GameServiceRepository(),
                new LoginServiceRepository(),
                new DatabaseServiceRepository()
            });
            configServer.ChangeValueSection("NameServer", "LoginServer");
            configServer.ChangeValueSection("IPAddressServer", IPAddress.Parse("192.168.1.100"));
            configServer.ChangeValueSection("PortServer", 2200);
            configServer.ChangeValueSection("ConfigCommunicationServices", new List<ConfigCommunicationClient>()
            {       
                new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3200),
                new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3201),
                new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3202),
            });
            loginServer = BuilderServer.CreateServer(configServer);
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            loginServer.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            loginServer.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            loginServer.Run();
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
