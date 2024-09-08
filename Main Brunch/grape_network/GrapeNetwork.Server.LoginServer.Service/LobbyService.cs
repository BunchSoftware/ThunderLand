using GrapeNetwork.Protocol.LoginProtocol.Command.Authentication;
using GrapeNetwork.Protocol.LoginProtocol.Command.Lobby;
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
    public class LobbyService : Core.Service
    {
        public LobbyService(string nameService) : base(nameService)
        {
        }

        protected override void DistrubuteApplicationCommand(ApplicationCommand applicationCommand, ClientState clientState)
        {
            Action<ApplicationCommand> action = (command) => { SendApplicationCommand(command); };
            switch (applicationCommand.Command)
            {
                case 7:
                    {
                        RequestToGameServerForUserConnection command = new RequestToGameServerForUserConnection(applicationCommand.GroupCommand, applicationCommand.Command, applicationCommand.NameService);
                        command.CommandData = applicationCommand.CommandData;
                        command.Connection = applicationCommand.Connection;
                        command.Execute(new object[] { clientState, server, action });
                        break;
                    }
            }
        }
    }
}
