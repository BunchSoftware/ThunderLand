using GrapeNetwork.Configurator.Interfaces;
using GrapeNetwork.Console.Common;
using GrapeNetwork.Protocol.GameProtocol;
using GrapeNetwork.Server.BuilderServer;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Core.Grpc;
using GrapeNetwork.Server.Core.Protocol;
using GrapeNetwork.Server.Database.Service;
using GrapeNetwork.Server.Game.Service;
using GrapeNetwork.Server.Login.Service;
using System;
using System.Collections.Generic;
using System.Net;

namespace GrapeNetwork.Console.GameServer
{
    class Program
    {
        static Server.Core.Server gameServer = new Server.Core.Server();
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Game Server");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);

            ConfigService configAccountService = new ConfigService();
            configAccountService.ChangeValueSection("NameService", "AccountService");

            ConfigService configMapService = new ConfigService();
            configAccountService.ChangeValueSection("NameService", "MapService");

            ConfigServer configServer = new ConfigServer();
            configServer.ChangeValueSection("ApplicationProtocol", new GameProtocol());
            configServer.ChangeValueSection("InternalServicesConfig", new List<ConfigService>()
            {
                 configAccountService,
                 configMapService
            });
            configServer.ChangeValueSection("RepositoryServices", new List<IRepository<Service>>()
            {
                new GameServiceRepository(),
                new LoginServiceRepository(),
                new DatabaseServiceRepository()
            });
            configServer.ChangeValueSection("NameServer", "GameServer");
            configServer.ChangeValueSection("IPAddressServer", IPAddress.Parse("192.168.1.100"));
            configServer.ChangeValueSection("PortServer", 2201);
            configServer.ChangeValueSection("ConfigCommunicationServices", new List<ConfigCommunicationClient>()
            {
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3200),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3201),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3202),
            });

            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            gameServer = BuilderServer.CreateServer(configServer);
            gameServer.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            gameServer.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            gameServer.Run();
            ConsoleManager.ReadKey();
            gameServer.Stop();
            ConsoleManager.ReadKey();
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            gameServer.Stop();
        }
    }
}
