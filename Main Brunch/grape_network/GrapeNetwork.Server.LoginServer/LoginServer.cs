using GrapeNetwork.Client.Core;
using GrapeNetwork.Core.Server;
using GrapeNetwork.Packages;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.LoginServer
{
    public class LoginServer : BaseServer
    {
        public List<ClientState> clientStates = new List<ClientState>(); 

        public override void Run()
        {
            base.Run();
            List<PackageProcessingCondition> packageProcessingConditions = new List<PackageProcessingCondition>() 
            {
                new PackageProcessingCondition(0, 0),
                new PackageProcessingCondition(0, 1),
                new PackageProcessingCondition(0, 4),
                new PackageProcessingCondition(0, 7)
            };
            transportServer.SetCondition(packageProcessingConditions);
            transportServer.OnRecieveDataClient += (connection, package) =>
            {
                switch (package.Command)
                {
                    case 0:
                        break;
                    case 1:
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            BinaryReader binaryReader = new BinaryReader(memoryStream);
                            memoryStream.Write(package.Body);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            string login = binaryReader.ReadString();
                            string password = binaryReader.ReadString();
                            if (login == "Den4o" && password == "win")
                            {
                                clientStates.Add(new ClientState());
                                DebugInfo($"Клиент {connection.RemoteAdressClient} направлен в лобби");
                                Package packageLobby = new Package()
                                {
                                    GroupCommand = 0,
                                    Command = 2
                                };
                                transportServer.SendPackage(connection, packageLobby);
                            }
                            else
                            {
                                Package packageRejection = new Package()
                                {
                                    GroupCommand = 0,
                                    Command = 3
                                };
                                transportServer.SendPackage(connection, packageRejection);
                            }
                            break;
                        }
                    case 4:
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            BinaryReader binaryReader = new BinaryReader(memoryStream);
                            memoryStream.Write(package.Body);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            string login = binaryReader.ReadString();
                            string password = binaryReader.ReadString();
                            if (login == "Den4o" && password == "win")
                            {
                                clientStates.Add(new ClientState());
                                DebugInfo($"Такой клиент уже зарегистрирован !");
                                Package packageRehected = new Package()
                                {
                                    GroupCommand = 0,
                                    Command = 6
                                };
                                transportServer.SendPackage(connection, packageRehected);
                            }
                            else
                            {
                                DebugInfo($"Зарегистрирован новый акаунт !");
                                Package packageRejection = new Package()
                                {
                                    GroupCommand = 0,
                                    Command = 5
                                };
                                transportServer.SendPackage(connection, packageRejection);
                            }
                            break;
                        }
                    case 7:
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            BinaryReader binaryReader = new BinaryReader(memoryStream);
                            memoryStream.Write(package.Body);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            string ipAdress = binaryReader.ReadString();
                            string port = binaryReader.ReadString();
                            if (ipAdress == "192.168.1.100" && port == "2201")
                            {
                                DebugInfo($"Клиент {connection.RemoteAdressClient} был переадресован на игровой сервер");
                                Package packageRehected = new Package()
                                {
                                    GroupCommand = 0,
                                    Command = 9,
                                    ReconnectionOtherServer = true
                                };
                                transportServer.SendPackage(connection, packageRehected);
                            }
                            else
                            {
                                DebugInfo($"Игровой сервер отклонил запрос на подключение");
                                Package packageRejection = new Package()
                                {
                                    GroupCommand = 0,
                                    Command = 8
                                };
                                transportServer.SendPackage(connection, packageRejection);
                            }
                            break;
                        }
                    default:
                        break;
                }
            };
            transportServer.OnConnectedClient += (connection) =>
            {
                Package package = new Package();
                package.AuthAndGetRSAKey = true;
                package.Body = Encoding.UTF8.GetBytes("RSA Key");
                transportServer.SendPackage(connection, package);
            };
        }
        public override string ReadConfig()
        {
            return "config";
        }
        public override void Stop()
        {
            transportServer.Stop();
        }
    }
}
