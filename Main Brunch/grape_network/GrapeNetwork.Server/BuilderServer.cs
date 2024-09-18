
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.BuilderServer
{
    public class BuilderServer
    {
        public Core.Server CreateServer(ConfigServer configServer)
        {
            Core.Server server = new Core.Server();
            Configurator configurator = new Configurator();
            configServer.hostBuilder = CreateHostBuilderDatabase(new string[] { }, configServer.IPAdressServer, configServer.PortServer + 1000);
            server.ReadConfig(configServer);
            return server;
        }
        private IHostBuilder CreateHostBuilderDatabase(string[] args, IPAddress IPAddress, int Port) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress, Port);
                    });
                    webBuilder.UseStartup<StartupCommunication>();
                });     
    }
}
