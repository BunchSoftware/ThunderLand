using GrapeNetwork.Server.Core.Configuration;
using System.Collections.Generic;
using System.Net;

namespace GrapeNetwork.Server.BuilderServer
{
    public class CommunicationServiceData
    {
        public List<ConfigCommunicationClient> config  = new List<ConfigCommunicationClient>();
        public IPAddress IPAdress;
        public int Port;

        public CommunicationServiceData() { }
    }
}
