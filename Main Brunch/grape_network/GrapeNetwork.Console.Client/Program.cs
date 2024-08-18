using GrapeNetwork.Console.Common;
using GrapeNetwork.Client.Core;
using System;
using System.Threading;

namespace GrapeNetwork.Console.Client
{
    class Program
    {
        static GameClient gameClient = new GameClient(2200, "192.168.56.1");
        static bool isAuth = false;
        private async static void A()
        {
            //var httpHandler = new HttpClientHandler();
            //httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //using var channel = GrpcChannel.ForAddress("http://localhost:3000", new GrpcChannelOptions { HttpHandler = httpHandler });
            ////var client = new AuthenticationServiceGrcp.AuthenticationServiceGrcpClient(channel);
            ////var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
            //ConsoleManager.WriteLine(reply.Message);
            //ConsoleManager.WriteLine("Press any key to exit...");
        }
        private static void Main()
        {
            //A();
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

            gameClient.ConnectToServer();

            while (isAuth == false)
            {
                Thread.Sleep(1000);
                Registration();
                Thread.Sleep(1000);
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
