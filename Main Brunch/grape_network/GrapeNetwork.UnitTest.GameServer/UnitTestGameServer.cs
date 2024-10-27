
using GrapeNetwork.Protocol.GameProtocol;
using GrapeNetwork.Server.BuilderServer;
using GrapeNetwork.Server.Core;
using GrapeNetwork.Server.Core.Configuration;
using GrapeNetwork.Server.Game.Service;
using System.Net;
using Xunit;

namespace GrapeNetwork.UnitTest.GameServer
{
    public class UnitTestGameServer
    {
        static Server.Core.Server gameServer = new Server.Core.Server();
        [Fact]
        public void TestBuilderServer()
        {
            ConfigServer configServer = new ConfigServer();
            configServer.ChangeValueSection("ApplicationProtocol", new GameProtocol());
            configServer.ChangeValueSection("InternalServices", new List<Service>()
            {
                 new AccountService("AccountService"),
                 new MapService("MapService")
            });
            configServer.ChangeValueSection("NameServer", "GameServer");
            configServer.ChangeValueSection("IPAddressServer", IPAddress.Parse("192.168.1.100"));
            configServer.ChangeValueSection("PortServer", 2201);
            configServer.ChangeValueSection("ConfigCommunicationServices", new List<ConfigCommunicationClient>()
            {
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3200),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3201),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3202),
            });
            gameServer = BuilderServer.CreateServer(configServer);
            gameServer.Run();
            gameServer.Stop();
        }
        [Fact]
        public void TestReadConfig()
        {
            ConfigServer configServer = new ConfigServer();
            configServer.ChangeValueSection("ApplicationProtocol", new GameProtocol());
            configServer.ChangeValueSection("InternalServices", new List<Service>()
            {
                 new AccountService("AccountService"),
                 new MapService("MapService")
            });
            configServer.ChangeValueSection("NameServer", "GameServer");
            configServer.ChangeValueSection("IPAddressServer", IPAddress.Parse("192.168.1.100"));
            configServer.ChangeValueSection("PortServer", 2201);
            configServer.ChangeValueSection("ConfigCommunicationServices", new List<ConfigCommunicationClient>()
            {
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3200),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3201),
                    new ConfigCommunicationClient(IPAddress.Parse("192.168.1.100"), 3202),
            });
            gameServer = new Server.Core.Server();
            gameServer.ReadConfig(configServer);
            gameServer.Run();
            gameServer.Stop();
        }
    }
}