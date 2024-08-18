using GrapeNetwork.Server.Core;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Lobby
{
    public class ResponseConnectingUserToGameServer : CommandProcessing
    {
        public ResponseConnectingUserToGameServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseConnectingUserToGameServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseConnectingUserToGameServer command)
        {
            throw new NotImplementedException();
        }
    }
}
