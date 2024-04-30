using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core.Command
{
    public class ResponseConnectToServer : CommandProcessing
    {
        public ResponseConnectToServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseConnectToServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseConnectToServer command)
        {
            throw new NotImplementedException();
        }
    }
}
