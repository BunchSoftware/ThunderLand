using GrapeNetwork.Server.Core;
using System;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Registration
{
    public class ResponseRejectedRegistrationUser : CommandProcessing
    {
        public ResponseRejectedRegistrationUser(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseRejectedRegistrationUser DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedRegistrationUser command)
        {
            throw new NotImplementedException();
        }
    }
}
