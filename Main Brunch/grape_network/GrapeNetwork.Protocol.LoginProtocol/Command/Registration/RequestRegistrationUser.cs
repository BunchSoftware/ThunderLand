using GrapeNetwork.Server.Core;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Registration
{
    public class RequestRegistrationUser : CommandProcessing
    {
        public RequestRegistrationUser(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static RequestRegistrationUser DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestRegistrationUser command)
        {
            throw new NotImplementedException();
        }
    }
}
