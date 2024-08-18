using GrapeNetwork.Server.Core;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Authentication
{
    public class ResponseSendClientToLobby : CommandProcessing
    {
        public ResponseSendClientToLobby(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseSendClientToLobby DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseSendClientToLobby command)
        {
            throw new NotImplementedException();
        }
    }
}
