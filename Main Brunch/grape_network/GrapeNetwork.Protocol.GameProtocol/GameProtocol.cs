using GrapeNetwork.Core;
using GrapeNetwork.Packages;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GrapeNetwork.Protocol.GameProtocol
{
    public class GameProtocol : TransportProtocol
    {
        protected List<CommandProcessing> commandRegistry = new List<CommandProcessing>();

        public GameProtocol(List<CommandProcessing> commandRegistry)
        {
            this.commandRegistry = commandRegistry;
        }

        public void CreatePackage(Package package)
        {
            outputQueueTransportPackage.Enqueue(package);
        }
        public CommandProcessing GetLastCommandProcessing()
        {
            Package package = GetLastPackage();
            CommandProcessing commandProcessing = null;
            foreach (CommandProcessing command in commandRegistry)
            {
                if (command.GroupCommand == package.GroupCommand && command.Command == package.Command)
                {
                    commandProcessing = new CommandProcessing(package.GroupCommand, package.Command, command.NameService);
                    commandProcessing.CommandData = package.Body;
                }
            }
            return commandProcessing;
        }
    }
}
