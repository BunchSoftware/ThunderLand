using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Protocol.LoginProtocol.Command.Lobby
{
    public class RequestToGameServerForUserConnection : CommandProcessing
    {
        public RequestToGameServerForUserConnection(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(ClientState clientState)
        {

        }
        public static RequestToGameServerForUserConnection DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestToGameServerForUserConnection command)
        {
            throw new NotImplementedException();
        }
    }
}
