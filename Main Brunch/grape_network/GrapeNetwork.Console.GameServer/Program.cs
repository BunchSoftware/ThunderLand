using GrapeNetwork.Console.Common;
using GrapeNetwork.Protocol.GameProtocol;
using GrapeNetwork.Server.BuilderServer;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Configuration;
using System;
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
            ConfigServer configServer = new ConfigServer()
            {
                Services = new System.Collections.Generic.List<Service>()
                {
                    new Server.Game.Service.AccountService("AccountService"),
                    new Server.Game.Service.MapService("MapService")
                },
                ConfigServices = new System.Collections.Generic.List<ConfigService>()
                {
                    new ConfigService(),
                    new ConfigService()
                },
                ApplicationProtocol = new GameProtocol(),
                NameServer = "GameServer",
                PortServer = 2201,
                IPAdressServer = IPAddress.Parse("192.168.1.100"),

                ConfigCommunicationServices = new System.Collections.Generic.List<ConfigCommunicationService>()
                {
                    new ConfigCommunicationService(IPAddress.Parse("192.168.1.100"), 3200),
                    new ConfigCommunicationService(IPAddress.Parse("192.168.1.100"), 3201),
                    new ConfigCommunicationService(IPAddress.Parse("192.168.1.100"), 3202),
                },
            };
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
