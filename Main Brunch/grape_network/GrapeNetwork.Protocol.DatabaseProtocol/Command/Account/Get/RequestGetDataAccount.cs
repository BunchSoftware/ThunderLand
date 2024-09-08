using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.IO;

namespace GrapeNetwork.Protocol.DatabaseProtocol.Command.Account.Get
{
    public class RequestGetDataAccount : ApplicationCommand
    {
        public RequestGetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            ClientState clientState = data[0] as ClientState;
            Server.Core.Server server = data[1] as Server.Core.Server;
            Action<ApplicationCommand> action = data[2] as Action<ApplicationCommand>;

            if (server != null && clientState != null)
            {

            }
        }
        public static RequestGetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestGetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
