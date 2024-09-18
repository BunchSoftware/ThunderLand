using GrapeNetwork.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.Server.Core.Protocol
{
    public class ApplicationProtocol : TransportProtocol
    {
        protected List<ApplicationCommand> commandRegistry;

        public void CreatePackage(Package package)
        {
            outputQueueTransportPackage.Enqueue(package);
        }
        public ApplicationCommand GetLastCommandProcessing()
        {
            Package package = GetLastPackage();
            ApplicationCommand commandProcessing = null;
            foreach (ApplicationCommand command in commandRegistry)
            {
                if (command.GroupCommand == package.GroupCommand && command.Command == package.Command)
                {
                    commandProcessing = new ApplicationCommand(package.GroupCommand, package.Command, command.NameService);
                    commandProcessing.CommandData = package.Body;
                }
            }
            return commandProcessing;
        }
    }
}
