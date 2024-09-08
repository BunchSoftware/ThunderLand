using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.DatabaseProtocol.Command.Account.Get
{
    public class ResponseRejectedGetDataAccount : ApplicationCommand
    {
        public ResponseRejectedGetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {

        }
        public static ResponseRejectedGetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedGetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
