using GrapeNetwork.Server.Core;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Lobby
{
    public class RequestToGameServerForUserConnection : CommandProcessing
    {
        public RequestToGameServerForUserConnection(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

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
