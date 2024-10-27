
using GrapeNetwork.Console.Common;
using GrapeNetwork.Server.Communication.Services;
using GrapeNetwork.Server.Core.Configuration;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        public static Core.Server CreateServer(ConfigServer configServer)
        {
            bool isBuild = false;
            Core.Server server = new Core.Server();

            Core.Grpc.GrpcServer grpcServer = new Core.Grpc.GrpcServer();
            ConfigGrpcServer configGrpcServer = new ConfigGrpcServer();
            configGrpcServer.ChangeValueSection("IPAddressServer", configServer.GetSection<IPAddress>("IPAddressServer"));
            configGrpcServer.ChangeValueSection("PortServer", configServer.GetSection<int>("PortServer") + 1000);

            configServer.ChangeValueSection("GrpcServer", grpcServer);
            server.OnDebugInfo += (message) => {
                if (isBuild == false)
                    ConsoleManager.Debug(message);
            };

            IPAddress IPAddressServer = configServer.GetSection<IPAddress>("IPAddressServer");
            int PortServer = configServer.GetSection<int>("PortServer");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(new string[] { });
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddressServer, PortServer+1000);
            });
            builder.Services.AddGrpc();
            CommunicationServiceData data = new CommunicationServiceData()
            {
                config = configServer.GetSection<List<ConfigCommunicationClient>>("ConfigCommunicationServices"),
                IPAdress = IPAddressServer,
                Port = PortServer + 1000
            };
            builder.Services.AddSingleton(new CommunicationService(data));
            WebApplication app = builder.Build();
            IConfiguration configuration = app.Configuration;

            app.MapGrpcService<CommunicationService>();
            app.MapGet("/", () => "Hello World!");

            Task RunAsync = new Task(() => 
            {
                app.RunAsync();
            });
            Task StopAsync = new Task(() =>
            {
                app.StopAsync();
            });
            configGrpcServer.ChangeValueSection("RunAsync", RunAsync);
            configGrpcServer.ChangeValueSection("StopAsync", StopAsync);

            grpcServer.ReadConfig(configGrpcServer);
            server.ReadConfig(configServer);

            isBuild = true;

            return server;
        }       
    }
}
