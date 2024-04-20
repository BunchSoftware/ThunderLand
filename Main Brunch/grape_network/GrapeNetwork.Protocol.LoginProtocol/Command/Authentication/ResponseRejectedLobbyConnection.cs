using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Authentication
{
    public class ResponseRejectedLobbyConnection : CommandProcessing
    {
        public ResponseRejectedLobbyConnection(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseRejectedLobbyConnection DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedLobbyConnection command)
        {
            throw new NotImplementedException();
        }
    }
}
