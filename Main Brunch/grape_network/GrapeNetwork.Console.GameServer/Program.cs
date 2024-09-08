using GrapeNetwork.Console.Common;
using GrapeNetwork.Protocol.GameProtocol;
using GrapeNetwork.Server.BuilderServer;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.Net;

namespace GrapeNetwork.Console.GameServer
{
    class Program
    {
        static Server.Core.Server gameServer;
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Game Server");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);
            BuilderServer builderServer = new BuilderServer();
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
            };
            gameServer = builderServer.CreateServer(configServer, TypeProtocol.GameProtocol);
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            gameServer.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            gameServer.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            gameServer.Run();
            ConsoleManager.Debug("Сервер запущен");
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
