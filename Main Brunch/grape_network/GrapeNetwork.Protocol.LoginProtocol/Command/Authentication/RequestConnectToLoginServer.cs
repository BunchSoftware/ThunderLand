using GrapeNetwork.Server.Core;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Authentication
{
    public class RequestConnectToLoginServer : CommandProcessing
    {
        public RequestConnectToLoginServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

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
