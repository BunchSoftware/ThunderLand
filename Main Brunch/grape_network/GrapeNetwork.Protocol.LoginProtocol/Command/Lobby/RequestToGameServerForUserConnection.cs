using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Lobby
{
    public class RequestToGameServerForUserConnection : ApplicationCommand
    {
        public RequestToGameServerForUserConnection(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            ClientState clientState = data[0] as ClientState;
            Server.Core.Server server = data[1] as Server.Core.Server;
            Action<ApplicationCommand> action = data[2] as Action<ApplicationCommand>;

            if (clientState.isAuth && clientState.isLobby)
            {
                server.DebugInfo($"Клиент {clientState.connection.RemoteAdressClient} был переадресован на игровой сервер");
                ResponseConnectingUserToGameServer commandProcessingConnectingUserToGameServer = new ResponseConnectingUserToGameServer(1, 9, "LobbyService");
                commandProcessingConnectingUserToGameServer.Connection = clientState.connection;
                action.Invoke(commandProcessingConnectingUserToGameServer);
            }
            else
            {
                server.DebugInfo($"Ошибка. Игрок {clientState.connection.RemoteAdressClient} не находится в лобби");
                ResponseRejectedUserConnectionToGameServer commandProcessingRejectedUserConnectionToGameServer = new ResponseRejectedUserConnectionToGameServer(1, 8, "LobbyService");
                commandProcessingRejectedUserConnectionToGameServer.Connection = clientState.connection;
                action.Invoke(commandProcessingRejectedUserConnectionToGameServer);
            }
        }
        public static RequestToGameServerForUserConnection DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestToGameServerForUserConnection command)
        {
            throw new NotImplementedException();
        }
    }
}
