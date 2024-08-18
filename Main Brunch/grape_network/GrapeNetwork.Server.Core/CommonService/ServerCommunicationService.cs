using GrapeNetwork.Core.Client;
using GrapeNetwork.Packages;
using System.IO;
using System.Net;

namespace GrapeNetwork.Server.Core.CommonService
{
    public class ServerCommunicationService : BaseService
    {
        protected TransportClient transportClient;

        public ServerCommunicationService(string nameService) : base(nameService)
        {
            
        }

        protected override void DistrubuteCommandProcessing(CommandProcessing commandProcessing, ClientState clientState)
        {
            switch (commandProcessing.Command)
            {
                case 1:
                    {
                        if (transportClient == null)
                        {
                            if (commandProcessing.CommandData.Length > 0)
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                BinaryReader binaryReader = new BinaryReader(memoryStream);
                                memoryStream.Write(commandProcessing.CommandData);
                                memoryStream.Seek(0, SeekOrigin.Begin);

                                int PortClient = binaryReader.ReadInt32();
                                string IPAdressClient = binaryReader.ReadString();
                                int PortServer = binaryReader.ReadInt32();
                                string IPAdressServer = binaryReader.ReadString();

                                TransportClient transportClient = new TransportClient(PortClient, IPAddress.Parse(IPAdressClient));
                                transportClient.ConnectToServer(PortServer, IPAddress.Parse(IPAdressServer));
                                commandProcessing.CommandData = null;

                                BinaryWriter writer = new BinaryWriter(memoryStream);
                                memoryStream.SetLength(0);
                                writer.Write(2200);
                                writer.Write("192.168.1.100");
                                writer.Write(2201);
                                writer.Write("192.168.1.100");

                                commandProcessing.CommandData = memoryStream.ToArray();

                                Package package = new Package()
                                {
                                    IPConnection = Package.ConvertFromIpAddressToInteger("192.168.1.100"),
                                    GroupCommand = commandProcessing.GroupCommand,
                                    Command = commandProcessing.Command,
                                    Body = commandProcessing.CommandData
                                };

                                server.DebugInfo($"Подключен к TL {IPAdressServer}:{PortServer}");

                                transportClient.SendPackage(package, false);

                                this.transportClient = transportClient;
                            }
                        }
                        break;
                    }
            };
        }

        public override void ReadConfig()
        {
            base.ReadConfig();
        }
    }
}
