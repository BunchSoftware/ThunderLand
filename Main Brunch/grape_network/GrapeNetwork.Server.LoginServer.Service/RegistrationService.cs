using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Protocol.LoginProtocol.Command.Registration;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Login.Service
{
    public class RegistrationService : Core.Service
    {
        public RegistrationService(string nameService) : base(nameService)
        {
        }

        protected override void DistrubuteApplicationCommand(ApplicationCommand applicationCommand, ClientState clientState)
        {
            Action<ApplicationCommand> action = (command) => { SendApplicationCommand(command); };
            switch (applicationCommand.Command)
            {
                case 4:
                    {
                        RequestRegistrationUser command = new RequestRegistrationUser(applicationCommand.GroupCommand, applicationCommand.Command, applicationCommand.NameService);
                        command.CommandData = applicationCommand.CommandData;
                        command.Connection = applicationCommand.Connection;
                        command.Execute(new object[] { clientState, server, action });
                        break;
                    }
            }
        }
    }
}
