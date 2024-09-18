
using System.Net;

namespace GrapeNetwork.Server.BuilderServer
{
    public class CommunicationClient
    {
        public readonly Grape.Communication.CommunicationClient communicationClient;
        public readonly IPAddress IPAddress;
        public readonly int Port;

        public CommunicationClient(Grape.Communication.CommunicationClient communicationClient, IPAddress IPAddress, int Port)
        {
            this.IPAddress = IPAddress;
            this.Port = Port;
        }
    }
}
