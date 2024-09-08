using Grape;
using GrapeNetwork.Server.Core.Configuration;
using Grpc.Net.Client;
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
        public Core.Server CreateServer(ConfigServer configServer, TypeProtocol typeProtocol)
        {
            Core.Server server = new Core.Server();
            Configurator configurator = new Configurator();
            switch (typeProtocol)
            {
                case TypeProtocol.LoginProtocol:
                    configServer.hostBuilder = CreateHostBuilderLogin(new string[] { }, configServer.IPAdressLogin, configServer.PortLogin + 1000);
                    break;
                case TypeProtocol.GameProtocol:
                    configServer.hostBuilder = CreateHostBuilderGame(new string[] { }, configServer.IPAdressGame, configServer.PortGame + 1000);
                    break;
                case TypeProtocol.DatabaseProtocol:
                    configServer.hostBuilder = CreateHostBuilderDatabase(new string[] { }, configServer.IPAdressDatabase, configServer.PortDatabase + 1000);
                    break;
            }
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
                    webBuilder.UseStartup<StartupDatabase>();
                });
        private IHostBuilder CreateHostBuilderGame(string[] args, IPAddress IPAddress, int Port) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress, Port);
                    });
                    webBuilder.UseStartup<StartupGame>();
                });
        private IHostBuilder CreateHostBuilderLogin(string[] args, IPAddress IPAddress, int Port) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress, Port);
                    });
                    webBuilder.UseStartup<StartupLogin>();
                });
        public static void Init(TypeProtocol typeProtocol, IPAddress IPAddress, int Port)
        {
            GrpcChannel grpcChannel = GrpcChannel.ForAddress("http://" + IPAddress.ToString() + ":" + Port + 1000);
            switch (typeProtocol)
            {
                case TypeProtocol.LoginProtocol:
                    Login.LoginClient loginClient = new Login.LoginClient(grpcChannel);
                    break;
                case TypeProtocol.GameProtocol:
                    Game.GameClient gameClient = new Game.GameClient(grpcChannel);
                    break;
                case TypeProtocol.DatabaseProtocol:
                    Database.DatabaseClient databaseClient = new Database.DatabaseClient(grpcChannel);
                    break;
            }
        }
    }
}
