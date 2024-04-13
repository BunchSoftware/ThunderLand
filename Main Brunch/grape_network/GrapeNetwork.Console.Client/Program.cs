using GrapeNetwork.Core.Client;
using GrapeNetwork.Packages;
using NLog.Fluent;
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
        static TransportClient transportClient = new TransportClient();
        static bool isAuth = false;
        private static void Main()
        {
            ConsoleManager.WriteLine("Запуск TL Client");
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;           
            ConsoleManager.SkipLine(1);
            transportClient.OnDebugInfo += (message) =>
            {
                ConsoleManager.Debug(message);
            };
            transportClient.OnExceptionInfo += (exception) =>
            {
                ConsoleManager.Error(exception);
            };
            transportClient.OnRecieveDataEvent += (package) =>
            {
                switch (package.Command)
                {
                    case 2:
                        {
                            ConsoleManager.Debug("Авторизация на сервере прошла успешна, просим напрвиться в лобби");
                            ConsoleManager.Debug("Вход в лобби");
                            ConsoleManager.Debug("Вход в лобби произошел успешно !");
                            isAuth = true;
                            break;
                        }
                    case 3:
                        {
                            ConsoleManager.WriteLineAndEditColor("Логин или пароль были не верны, проверьте правильность вводимых данных", ConsoleColor.Red);
                            break;
                        }
                    case 5:
                        {
                            ConsoleManager.Debug("Регистрация прошла успешна ! Желаем хорошой игры !");
                            break;
                        }
                    case 6:
                        {
                            ConsoleManager.WriteLineAndEditColor("Такой акаунт уже зарегистрирован", ConsoleColor.Red);
                            break;
                        }
                    case 8:
                        {
                            ConsoleManager.WriteLineAndEditColor("Запрос игровым сервером был отклонен, попробуйте еще раз !", ConsoleColor.Red);
                            break;
                        }
                    case 9:
                        {
                            ConsoleManager.Debug("Игровой сервер принял вызов !");
                            ConsoleManager.Debug("Вход в игровой мир");
                            ConsoleManager.Debug("Вход в игровой мир произошел успешно !");
                            break;
                        }
                    default:
                        break;
                }
            };
            ConsoleManager.Write("Введите адрес логин сервера: ");
            string ipAdressServer = System.Console.ReadLine();
            ConsoleManager.Write("Введите порт логин сервера: ");
            string portServer = System.Console.ReadLine();
            transportClient.ConnectToServer(int.Parse(portServer), IPAddress.Parse(ipAdressServer));
            while (isAuth == false)
            {
                //Registration();
                //Thread.Sleep(1000);
                Auth();
                Thread.Sleep(1000);
                ConsoleManager.WriteLine("Зайти на игровой сервер ? (Y/N)");
                string request = System.Console.ReadLine();
                if(request == "Y")
                {
                    MemoryStream memoryStream = new MemoryStream();
                    BinaryWriter writer = new BinaryWriter(memoryStream);
                    writer.Write("192.168.1.100");
                    writer.Write("2201");
                    Package package = new Package()
                    {
                        IPConnection = 123,
                        IDConnection = 123,
                        GroupCommand = 0,
                        ReconnectionOtherServer = true,
                        Command = 7,
                        Body = memoryStream.ToArray()
                    };
                    transportClient.SendPackage(package, true);
                }
                Thread.Sleep(1000);
            }
            System.Console.ReadKey();
            System.Console.ReadKey();
            System.Console.ReadKey();
        }
        private static void ProcessExit(object sender, EventArgs e)
        {
            transportClient.DisconnectToServer();
        }
        private static void Auth()
        {
            ConsoleManager.WriteLine("Вход на сервер");
            ConsoleManager.Write("Введите логин учетной записи: ");
            string login = System.Console.ReadLine();
            ConsoleManager.Write("Введите пароль учетной записи: ");
            string password = System.Console.ReadLine();
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(login);
            writer.Write(password);
            Package package = new Package()
            {
                IPConnection = 123,
                IDConnection = 123,
                GroupCommand = 0,
                Command = 1,
                Body = memoryStream.ToArray()
            };
            transportClient.SendPackage(package, true);
        }
        private static void Registration()
        {
            ConsoleManager.WriteLine("Регистрация");
            ConsoleManager.Write("Введите логин учетной записи: ");
            string login = System.Console.ReadLine();
            ConsoleManager.Write("Введите пароль учетной записи: ");
            string password = System.Console.ReadLine();
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Write(login);
            writer.Write(password);
            Package package = new Package()
            {
                IPConnection = 123,
                IDConnection = 123,
                GroupCommand = 0,
                Command = 4,
                Body = memoryStream.ToArray()
            };
            transportClient.SendPackage(package, true);
        }
    }
}
