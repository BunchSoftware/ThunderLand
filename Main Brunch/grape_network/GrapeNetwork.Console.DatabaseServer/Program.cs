

using GrapeNetwork.Console.Common;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Core;
using System.Net;
using System;
using GrapeNetwork.Server.BuilderServer;
using GrapeNetwork.Protocol.DatabaseProtocol;

namespace GrapeNetwork.Console.DatabaseServer
{
    class Program
    {
        static Server.Core.Server databaseServer;
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Database Server");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);
            BuilderServer builderServer = new BuilderServer();
            ConfigServer configServer = new ConfigServer()
            {
                Services = new System.Collections.Generic.List<Service>()
                {
                    
                },
                ConfigServices = new System.Collections.Generic.List<ConfigService>()
                {
                    new ConfigService(),
                    new ConfigService()
                },
                ApplicationProtocol = new DatabaseProtocol(),
                NameServer = "DatabaseServer",
                PortServer = 2202,
                IPAdressServer = IPAddress.Parse("192.168.1.100"),
            };
            databaseServer = builderServer.CreateServer(configServer, TypeProtocol.DatabaseProtocol);
            databaseServer.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            databaseServer.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            databaseServer.Run();
            ConsoleManager.Debug("Сервер запущен");
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
