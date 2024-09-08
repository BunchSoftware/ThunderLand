using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.IO;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Authentication
{
    public class RequestConnectToLoginServer : ApplicationCommand
    {
        public RequestConnectToLoginServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            ClientState clientState = data[0] as ClientState;
            Server.Core.Server server = data[1] as Server.Core.Server;
            Action<ApplicationCommand> action = data[2] as Action<ApplicationCommand>;

            if (server != null && clientState != null)
            {
                MemoryStream memoryStream = new MemoryStream();
                BinaryReader binaryReader = new BinaryReader(memoryStream);
                memoryStream.Write(CommandData);
                memoryStream.Seek(0, SeekOrigin.Begin);
                string login = binaryReader.ReadString();
                string password = binaryReader.ReadString();

                if (login == "Den4o" && password == "win")
                {
                    server.DebugInfo($"Клиент {clientState.connection.RemoteAdressClient} направлен в лобби");
                    clientState.isAuth = true;
                    clientState.isLobby = true;
                    ResponseSendClientToLobby commandProcessingLobby = new ResponseSendClientToLobby(1, 2, "AuthenticationService");
                    commandProcessingLobby.Connection = clientState.connection;
                    action.Invoke(commandProcessingLobby);
                }
                else
                {
                    ResponseRejectedLobbyConnection commandProcessingLobbyRejection = new ResponseRejectedLobbyConnection(1, 3, "AuthenticationService");
                    commandProcessingLobbyRejection.Connection = clientState.connection;
                    action.Invoke(commandProcessingLobbyRejection);
                }
            }            
        }
        public static RequestConnectToLoginServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestConnectToLoginServer command)
        {
            throw new NotImplementedException();
        }
    }
}
