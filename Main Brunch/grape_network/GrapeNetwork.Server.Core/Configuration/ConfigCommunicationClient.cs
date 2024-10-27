using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GrapeNetwork.Server.Core.Configuration
{
    public class ConfigCommunicationClient
    {
        public int Port;
        public IPAddress IPAdress;

        public ConfigCommunicationClient(IPAddress IPAdress, int Port)
        {
            this.Port = Port;
            this.IPAdress = IPAdress;
        }
    }
}
