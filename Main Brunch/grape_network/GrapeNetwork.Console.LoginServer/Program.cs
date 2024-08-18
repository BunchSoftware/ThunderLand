using GrapeNetwork.Console.Common;
using System;

namespace GrapeNetwork.Console.LoginServer
{
    class Program
    {
        static Server.LoginServer.LoginServer loginServer = new Server.LoginServer.LoginServer();
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Login Server");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);
            loginServer.ReadConfig();
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
