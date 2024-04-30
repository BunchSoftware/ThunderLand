using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core.Command
{
    public class ResponseRejectedConnectToServer : CommandProcessing
    {
        public ResponseRejectedConnectToServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseRejectedConnectToServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedConnectToServer command)
        {
            throw new NotImplementedException();
        }
    }
}
