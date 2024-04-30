using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core.Command
{
    public class RequestConnectToServer : CommandProcessing
    {
        public RequestConnectToServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static RequestConnectToServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestConnectToServer command)
        {
            throw new NotImplementedException();
        }
    }
}
