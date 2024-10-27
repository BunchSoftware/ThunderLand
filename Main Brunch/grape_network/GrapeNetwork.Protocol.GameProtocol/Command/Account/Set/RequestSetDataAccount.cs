using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.IO;

namespace GrapeNetwork.Protocol.GameProtocol.Command.Account.Set
{
    public class RequestSetDataAccount : ApplicationCommand
    {
        public RequestSetDataAccount(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            ClientState clientState = data[0] as ClientState;
            Server.Core.Server server = data[1] as Server.Core.Server;
            Action<ApplicationCommand> action = data[2] as Action<ApplicationCommand>;

            if (server != null && clientState != null)
            {
                ResponseSetDataAccount responseGetDataAccount = new ResponseSetDataAccount(2, 2, "AccountService");
                responseGetDataAccount.Connection = clientState.connection;
                action.Invoke(responseGetDataAccount);
            }
        }
        public static RequestSetDataAccount DeserealizeCommand(byte[] data)
        {
            throw new NotImplementedException();
        }

        public static byte[] SerealizeCommand(RequestSetDataAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
