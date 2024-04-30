using GrapeNetwork.Client.Core;
using GrapeNetwork.Core.Client;
using GrapeNetwork.Packages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrapeNetwork.Console.Common
{
    class Program
    {
        static GameClient gameClient = new GameClient();
        static bool isAuth = false;
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Client");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;           
            ConsoleManager.SkipLine(1);
            gameClient.ReadConfig();
            gameClient.ConnectToServer();
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
            
            while (isAuth == false)
            {
                //Registration();
                //Thread.Sleep(1000);
                Auth();
                Thread.Sleep(1000);
                ConsoleManager.WriteLine("Зайти на игровой сервер ? (Y/N)");
                string request = System.Console.ReadLine();
                if (request == "Y")
                {
                    gameClient.EnterLobby();
                    isAuth = true;
                }
                Thread.Sleep(1000);
            }
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
