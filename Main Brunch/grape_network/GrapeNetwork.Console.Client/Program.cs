using GrapeNetwork.Console.Common;
using GrapeNetwork.Client.Core;
using System;
using System.Threading;

namespace GrapeNetwork.Console.Client
{
    class Program
    {
        static GameClient gameClient = new GameClient();
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Client");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
            ConsoleManager.SkipLine(1);
            gameClient.ReadConfig();
            gameClient.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            gameClient.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            gameClient.OnErrorInfo += (message) =>
            {
                ConsoleManager.WriteLineAndEditColor(message, ConsoleColor.Red);
            };

            gameClient.ConnectToLoginServer();
            gameClient.Authetication("Den4o", "win");
            gameClient.ConnectToGameServer();

            //while (isAuth == false)
            //{
            //    Thread.Sleep(1000);
            //    Registration();
            //    Thread.Sleep(1000);
            //        Auth();
            //        Thread.Sleep(1000);
            //    ConsoleManager.WriteLine("Зайти на игровой сервер ? (Y/N)");
            //    string request = System.Console.ReadLine();
            //    if (request == "Y")
            //    {
            //        gameClient.ConnectToGameServer();
            //        isAuth = true;
            //    }
            //    Thread.Sleep(1000);
            //}
            System.Console.ReadKey();
            System.Console.ReadKey();
            System.Console.ReadKey();
        }
        private static void ProcessExit(object sender, EventArgs e)
        {
           gameClient.DisconnectFromServer();
        }
        private static void Auth()
        {
            ConsoleManager.WriteLine("Вход на сервер");
            ConsoleManager.Write("Введите логин учетной записи: ");
            string login = System.Console.ReadLine();
            ConsoleManager.Write("Введите пароль учетной записи: ");
            string password = System.Console.ReadLine();
            gameClient.Authetication(login, password);
        }
        private static void Registration()
        {
            ConsoleManager.WriteLine("Регистрация");
            ConsoleManager.Write("Введите логин учетной записи: ");
            string login = System.Console.ReadLine();
            ConsoleManager.Write("Введите пароль учетной записи: ");
            string password = System.Console.ReadLine();
            gameClient.Registration(login, password);
        }
    }
}
