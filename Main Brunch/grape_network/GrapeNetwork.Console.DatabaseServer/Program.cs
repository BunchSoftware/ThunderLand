using GrapeNetwork.Console.Common;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Core;
using System.Net;
using System;
using GrapeNetwork.Protocol.DatabaseProtocol;
using GrapeNetwork.Server.BuilderServer;
using System.Collections.Generic;
using GrapeNetwork.Configurator.Interfaces;
using GrapeNetwork.Server.Database.Service;
using GrapeNetwork.Server.Game.Service;
using GrapeNetwork.Server.Login.Service;

namespace GrapeNetwork.Console.DatabaseServer
{
    class Program
    {
        static Server.Core.Server databaseServer = new Server.Core.Server();
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Database Server");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);

            ConfigServer configServer = new ConfigServer();
            configServer.ChangeValueSection("ApplicationProtocol", new DatabaseProtocol());
            ConfigService configAccountService = new ConfigService();
            configAccountService.ChangeValueSection("NameService", "AccountService");
            configServer.ChangeValueSection("InternalServicesConfig", new List<ConfigService>()
            {
                configAccountService
            });
            configServer.ChangeValueSection("RepositoryServices", new List<IRepository<Service>>()
            {
                new GameServiceRepository(),
                new LoginServiceRepository(),
                new DatabaseServiceRepository()
            });
            configServer.ChangeValueSection("NameServer", "DatabaseServer");
            configServer.ChangeValueSection("IPAddressServer", IPAddress.Parse("192.168.1.100"));
            configServer.ChangeValueSection("PortServer", 2202);
            configServer.ChangeValueSection("ConfigCommunicationServices", new List<ConfigCommunicationClient>()
            {
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3200),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3201),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3202),
            });

            databaseServer = BuilderServer.CreateServer(configServer);
            databaseServer.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            databaseServer.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            databaseServer.Run();
            ConsoleManager.ReadKey();
            databaseServer.Stop();
            ConsoleManager.ReadKey();
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            databaseServer.Stop();
        }
    }
}
