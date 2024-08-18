using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.LoginServer.Service
{
    public class LobbyService : BaseService
    {
        public LobbyService(string nameService) : base(nameService)
        {
        }

        protected override void DistrubuteCommandProcessing(CommandProcessing commandProcessing, ClientState clientState)
        {
            switch (commandProcessing.Command)
            {
                case 7:
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        BinaryReader binaryReader = new BinaryReader(memoryStream);
                        memoryStream.Write(commandProcessing.CommandData);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        string ipAdress = binaryReader.ReadString();
                        string port = binaryReader.ReadString();
                        if (ipAdress == "192.168.1.100" && port == "2201")
                        {
                            server.DebugInfo($"Клиент {clientState.connection.RemoteAdressClient} был переадресован на игровой сервер");
                            ResponseConnectingUserToGameServer commandProcessingConnectingUserToGameServer = new ResponseConnectingUserToGameServer(1, 9, "LobbyService");
                            commandProcessingConnectingUserToGameServer.Connection = clientState.connection;
                            SendCommandProcessing(commandProcessingConnectingUserToGameServer);
                        }
                        else
                        {
                            server.DebugInfo($"Игровой сервер отклонил запрос на подключение");
                            ResponseRejectedUserConnectionToGameServer commandProcessingRejectedUserConnectionToGameServer = new ResponseRejectedUserConnectionToGameServer(1, 8, "LobbyService");
                            commandProcessingRejectedUserConnectionToGameServer.Connection = clientState.connection;
                            SendCommandProcessing(commandProcessingRejectedUserConnectionToGameServer);
                        }
                        break;
                    }
            };
        }
    }
}
