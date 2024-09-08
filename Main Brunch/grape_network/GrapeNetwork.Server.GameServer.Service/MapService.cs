using GrapeNetwork.Protocol.GameProtocol.Command.Account.Get;
using GrapeNetwork.Server.Core.Protocol;
using GrapeNetwork.Server.Core;
using System;
using System.Collections.Generic;
using System.Text;
using GrapeNetwork.Server.Type.MapService;
using System.IO;

namespace GrapeNetwork.Server.Game.Service
{
    public class MapService : Core.Service
    {
        private World world = new World();

        public MapService(string nameService) : base(nameService)
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
                case 7:
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        BinaryReader binaryReader = new BinaryReader(memoryStream);
                        memoryStream.Write(commandProcessing.CommandData);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        int IDLocation = binaryReader.ReadInt32();

                        Location location = world.FindLocationByID(IDLocation);
                        location.OnLocationChanged += () =>
                        {
                            UpdateWorldState(clientState, location);
                        };

                        RequestGetWorldState command = new RequestGetWorldState(commandProcessing.GroupCommand, commandProcessing.Command, commandProcessing.NameService);
                        command.CommandData = commandProcessing.CommandData;
                        command.Connection = commandProcessing.Connection;
                        command.Execute(new object[] { clientState, server, action, world });
                        break;
                    }
            }
        }

        private void UpdateWorldState(ClientState clientState, Location location)
        {
            
        }
    }
}
