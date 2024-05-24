using GrapeNetwork.Packages;
using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Server.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.LoginServer.Service
{
    public class AuthenticationService : BaseService
    {
        public AuthenticationService(string nameService) : base(nameService)
        {

        }

        public override void Init(BaseServer server)
        {
            base.Init(server);
        }

        protected override void DistrubuteCommandProcessing(CommandProcessing commandProcessing, ClientState clientState)
        {
            switch (commandProcessing.Command)
            {
                case 1:
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        BinaryReader binaryReader = new BinaryReader(memoryStream);
                        memoryStream.Write(commandProcessing.CommandData);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        string login = binaryReader.ReadString();
                        string password = binaryReader.ReadString();
                        if (login == "Den4o" && password == "win")
                        {
                            server.DebugInfo($"Клиент {clientState.connection.RemoteAdressClient} направлен в лобби");
                            ResponseSendClientToLobby commandProcessingLobby = new ResponseSendClientToLobby(1, 2, "AuthenticationService");
                            commandProcessingLobby.Connection = clientState.connection;
                            SendCommandProcessing(commandProcessingLobby);
                        }
                        else
                        {
                            ResponseRejectedLobbyConnection commandProcessingLobbyRejection = new ResponseRejectedLobbyConnection(1, 3, "AuthenticationService");
                            commandProcessingLobbyRejection.Connection = clientState.connection;
                            SendCommandProcessing(commandProcessingLobbyRejection);
                        }
                        break;                                  
            }
        };
    }

        public override void ReadConfig()
        {
            base.ReadConfig();
        }
    }
}
