using GrapeNetwork.Server.Core.Protocol;
using System;

namespace GrapeNetwork.Protocol.DatabaseProtocol.Command.Account.Set
{
    public class ResponseSetDataAccount : ApplicationCommand
    {
        public ResponseSetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {

        }
        public static ResponseSetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseSetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
