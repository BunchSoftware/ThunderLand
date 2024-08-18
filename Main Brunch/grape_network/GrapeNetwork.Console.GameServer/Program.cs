using GrapeNetwork.Console.Common;
using System;

namespace GrapeNetwork.Console.GameServer
{
    class Program
    {
        static Server.GameServer.GameServer gameServer = new Server.GameServer.GameServer();
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Game Server");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);
            gameServer.ReadConfig();
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
