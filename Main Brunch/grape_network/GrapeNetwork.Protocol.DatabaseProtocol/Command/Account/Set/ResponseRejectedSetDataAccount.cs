using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.DatabaseProtocol.Command.Account.Set
{
    public class ResponseRejectedSetDataAccount : ApplicationCommand
    {
        public ResponseRejectedSetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {

        }
        public static ResponseRejectedSetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedSetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
