using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using GrapeNetwork.Server.Core.Grpc;
using GrapeNetwork.Server.Core.Protocol;

namespace GrapeNetwork.Server.Core.Configuration
{
    public class ConfigServer
    {
        public ApplicationProtocol ApplicationProtocol;
        public List<Service> Services;
        public List<ConfigService> ConfigServices;
        public string NameServer;
        public IPAddress IPAdressServer;
        public int PortServer;
        public List<ConfigCommunicationService> ConfigCommunicationServices;
        public GrpcServer GrpcServer;
    }
    public class ConfigCommunicationService
    {
        public int Port;
        public IPAddress IPAdress;

        public ConfigCommunicationService(IPAddress IPAdress, int Port)
        {
            this.Port = Port;
            this.IPAdress = IPAdress;
        }
    }
}
