using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Server.Database.Service
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
                
            }
        }
    }
}
