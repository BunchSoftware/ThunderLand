using GrapeNetwork.Core.Server;
using System;

namespace GrapeNetwork.Server.Core
{
    public class CommandProcessing
    {
        public ushort GroupCommand = 0;
        public uint Command = 0;
        public byte[] CommandData = null;
        public string NameService { get; }
        public Connection Connection;
        public Action OnCommandProcessingComplete;


        public CommandProcessing(ushort GroupCommand, uint Command, string nameService)
        {
            this.GroupCommand = GroupCommand;
            this.Command = Command;
            this.NameService = nameService;
        }

        public CommandProcessing(ushort groupCommand, uint command)
        {
            GroupCommand = groupCommand;
            Command = command;
        }

        public virtual void Execute(ClientState clientState) 
        {
            OnCommandProcessingComplete?.Invoke();
        }
    }
}
