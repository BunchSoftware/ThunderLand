using GrapeNetwork.Core.Client;
using GrapeNetwork.Core.Server;
using GrapeNetwork.Packages;
using GrapNetwork.LogWriter;
using System.Net;
using System.Text;

namespace GrapeNetwork.UnitTest.GameServer
{
    public class UnitTestLoginServer
    {
        [Fact]
        public void TestStartServer()
        {
            TransportServer networkServer = new TransportServer(7777, IPAddress.Parse("192.168.1.100"));
            Assert.True(networkServer.Start(), "Server Start");
            networkServer.Stop();
        }
        [Fact]
        public void TestStopServer()
        {
            TransportServer networkServer = new TransportServer(7777, IPAddress.Parse("192.168.1.100"));
            networkServer.Stop();
            Assert.True(!networkServer.IsActive, "Server Stop");
        }
        [Fact]
        public void TestStartStopServer()
        {
            TransportServer networkServer = new TransportServer(7777, IPAddress.Parse("192.168.1.100"));
            networkServer.Start();
            networkServer.Stop();
            Assert.True(true);
        }
        [Fact]
        public void TestSendPackageServer()
        {
            //TransportServer transportServer = new TransportServer(7777, IPAddress.Parse("192.168.1.100"));
            //transportServer.OnDebugInfo += (message) =>
            //{
            //    LogManager logManager = new LogManager("E:\\Files\\GitHub\\ThunderLand\\Main Brunch\\logs\\game_server_log\\", "serverLog");
            //    logManager.Debug(message);
            //};
            //transportServer.OnExceptionInfo += (exception) =>
            //{
            //    LogManager logManager = new LogManager("E:\\Files\\GitHub\\ThunderLand\\Main Brunch\\logs\\game_server_log\\", "serverLog");
            //    logManager.Error(exception);
            //};
            //transportServer.Start();

            //List<PackageProcessingCondition> packageProcessingConditions = new List<PackageProcessingCondition>();

            //TransportClient networkClient = new TransportClient();
            //networkClient.ConnectToServer(7777, IPAddress.Parse("192.168.1.100"));
            //packageProcessingConditions.Add(new PackageProcessingCondition(0, 0));

            //networkClient.SetCondition(packageProcessingConditions);

            //Package package = new Package();

            //networkClient.OnRecieveDataEvent += (package) =>
            //{
            //    LogManager logManager = new LogManager("E:\\Files\\GitHub\\ThunderLand\\Main Brunch\\logs\\game_server_log\\", "serverLog");
            //    logManager.Debug(Encoding.UTF8.GetString(package.Body));
            //};

            //for (int i = 0; i < 10; i++)
            //{
            //    package.Body = Encoding.UTF8.GetBytes($"{i} Ура победа !");
            //    transportServer.SendPackage(networkClient.LocalAdressClient, package);
            //}

            //networkServer.Stop();
        }
        [Fact]
        public void TestDisposeServer()
        {
            TransportServer networkServer = new TransportServer(7777, IPAddress.Parse("192.168.1.100"));
            networkServer.Dispose();
            Assert.True(true);
        }
    }
}