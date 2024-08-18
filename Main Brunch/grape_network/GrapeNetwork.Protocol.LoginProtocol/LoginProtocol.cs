using GrapeNetwork.Core;
using GrapeNetwork.Packages;
using GrapeNetwork.Server.Core;
using System.Collections.Generic;

namespace GrapeNetwork.Protocol.LoginProtocol
{
    public class LoginProtocol : TransportProtocol
    {
        protected List<CommandProcessing> commandRegistry = new List<CommandProcessing>();

        public LoginProtocol(List<CommandProcessing> commandRegistry)
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
        //public virtual CommandProcessing GetLastCommandProcessing()
        //{
        //    CommandProcessing commandProcessing = new CommandProcessing();
        //    if (RecievePackageCount != 0)
        //        return commandProcessing;
        //    else
        //        return null;
        //}
    }
}
