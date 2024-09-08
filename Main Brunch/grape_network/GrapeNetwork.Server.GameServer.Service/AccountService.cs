using GrapeNetwork.Server.Core.Protocol;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.Text;
using GrapeNetwork.Protocol.GameProtocol.Command.Account.Get;

namespace GrapeNetwork.Server.Game.Service
{
    public class AccountService : Core.Service
    {
        public AccountService(string nameService) : base(nameService)
        {

        }

        public override void Init(Core.Server server)
        {
            base.Init(server);
        }

        protected override void DistrubuteApplicationCommand(ApplicationCommand commandProcessing, ClientState clientState)
        {
            Action<ApplicationCommand> action = (command) => { SendApplicationCommand(command); };
            switch (commandProcessing.Command)
            {
                case 1:
                    {
                        RequestGetDataAccount command = new RequestGetDataAccount(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                        command.CommandData = commandProcessing.CommandData;
                        command.Connection = commandProcessing.Connection;
                        command.Execute(new object[] { clientState, server, action });
                        break;
                    }

            }
        }
    }
}
