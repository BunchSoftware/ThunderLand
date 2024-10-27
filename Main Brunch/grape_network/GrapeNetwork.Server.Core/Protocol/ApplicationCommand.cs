using GrapeNetwork.Core.Server;
using System;
using System.Diagnostics.SymbolStore;

namespace GrapeNetwork.Server.Core.Protocol
{
    public class ApplicationCommand
    {
        public ushort GroupCommand = 0;
        public uint Command = 0;
        public byte[] CommandData = null;
        public string NameService { get; }
        public Connection Connection;
        public Action OnCommandProcessingComplete;


        public ApplicationCommand(ushort GroupCommand, uint Command, string nameService)
        {
            this.GroupCommand = GroupCommand;
            this.Command = Command;
            NameService = nameService;
        }

        public ApplicationCommand(ushort groupCommand, uint command)
        {
            GroupCommand = groupCommand;
            Command = command;
        }

        public virtual void Execute(object[] data)
        {
            OnCommandProcessingComplete?.Invoke();
        }
    }
}
