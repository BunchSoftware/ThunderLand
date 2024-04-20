using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Lobby
{
    public class ResponseRejectedUserConnectionToGameServer : CommandProcessing
    {
        public ResponseRejectedUserConnectionToGameServer(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static ResponseRejectedUserConnectionToGameServer DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(ResponseRejectedUserConnectionToGameServer command)
        {
            throw new NotImplementedException();
        }
    }
}
