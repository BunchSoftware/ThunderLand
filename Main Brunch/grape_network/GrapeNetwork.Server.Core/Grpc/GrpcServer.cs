using Grape;
using GrapeNetwork.Core.Server;
using GrapeNetwork.Server.Core.Protocol;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.Core.Grpc
{
    public class GrpcServer
    {
        public event Action<Package> OnRecieveData;
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnDebugInfo;

        private Communication.CommunicationClient client;
        private AsyncDuplexStreamingCall<Package, Package> call;

        private IPAddress IPAddressServer;
        private int PortServer;

        private Task RunAsync;
        private Task StopAsync;

        public void Run()
        {
            RunAsync.Start();
        }

        public void SendPackage(GrapeNetwork.Core.Package package, string nameService)
        {
            Package grpcPackage = new Package()
            {
                GroupCommand = package.GroupCommand,
                Command = package.Command,
                NameService = nameService,
                IPConnection = package.IPConnection,
            };

            call = client.SendPackage();
            call.RequestStream.WriteAsync(grpcPackage);
            call.RequestStream.CompleteAsync();
        }

        public void ReadConfig(Configuration.ConfigGrpcServer config)
        {
            IPAddressServer = config.GetSection<IPAddress>("IPAddressServer");
            PortServer = config.GetSection<int>("PortServer");
            RunAsync = config.GetSection<Task>("RunAsync");
            StopAsync = config.GetSection<Task>("StopAsync");

            if (IPAddressServer == null)
                throw new NullReferenceException();
            if(RunAsync == null)
                throw new NullReferenceException();
            if (StopAsync == null)
                throw new NullReferenceException();

            GrpcChannel channel = GrpcChannel.ForAddress($"http://{IPAddressServer}:{PortServer}");
            client = new Communication.CommunicationClient(channel);
        }
        public void Stop()
        {
            StopAsync.Start();
        }
        public void ExceptionInfo(Exception exception) { OnExceptionInfo?.Invoke(exception); }
        public void DebugInfo(string message) { OnDebugInfo?.Invoke(message); }
    }
}
