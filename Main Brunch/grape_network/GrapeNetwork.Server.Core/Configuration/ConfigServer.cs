using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using GrapeNetwork.Server.Core.Protocol;
using Microsoft.Extensions.Hosting;

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
        public IPAddress IPAdressLogin;
        public int PortLogin;
        public IPAddress IPAdressGame;
        public int PortGame;
        public IPAddress IPAdressDatabase;
        public int PortDatabase;
        public IHostBuilder hostBuilder;
    }
}
