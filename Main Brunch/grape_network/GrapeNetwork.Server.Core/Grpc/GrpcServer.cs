using Grape;
using GrapeNetwork.Core.Server;
using GrapeNetwork.Server.Core.Protocol;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace GrapeNetwork.Server.Core.Grpc
{
    public class GrpcServer
    {
        public event Action<Package> OnRecieveData;
        public event Action<Exception> OnExceptionInfo;
        public event Action<string> OnDebugInfo;

        private Communication.CommunicationClient client;
        private AsyncDuplexStreamingCall<Package, Package> call;

        public void Run()
        {

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
            GrpcChannel channel = GrpcChannel.ForAddress($"http://{config.IPAdressServer}:{config.PortServer}");
            client = new Communication.CommunicationClient(channel);
        }
        public void Stop()
        {

        }
        public void ExceptionInfo(Exception exception) { OnExceptionInfo?.Invoke(exception); }
        public void DebugInfo(string message) { OnDebugInfo?.Invoke(message); }
    }
}
