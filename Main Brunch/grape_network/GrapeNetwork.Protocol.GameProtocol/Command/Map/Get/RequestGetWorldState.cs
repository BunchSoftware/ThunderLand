using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.IO;

namespace GrapeNetwork.Protocol.GameProtocol.Command.Account.Get
{
    public class RequestGetWorldState : ApplicationCommand
    {
        public RequestGetWorldState(ushort GroupCommand, uint Command, string nameService) : base(GroupCommand, Command, nameService)
        {
        }

        public override void Execute(object[] data)
        {
            ClientState clientState = data[0] as ClientState;
            Server.Core.Server server = data[1] as Server.Core.Server;
            Action<ApplicationCommand> action = data[2] as Action<ApplicationCommand>;

            if (server != null && clientState != null)
            {              
                ResponseGetWorldState responseGetDataAccount = new ResponseGetWorldState(2, 8, "MapService");
                responseGetDataAccount.Connection = clientState.connection;
                action.Invoke(responseGetDataAccount);
            }
        }


        private void UpdateWorldState(ClientState clientState)
        {

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
