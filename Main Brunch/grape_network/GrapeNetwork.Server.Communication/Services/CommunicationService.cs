using Grape;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using Grpc.Core;
using GrapeNetwork.Server.Core.Configuration;
using Grpc.Net.Client;
using GrapeNetwork.Server.BuilderServer;
using System.Net;

namespace GrapeNetwork.Server.Communication.Services
{
    public class CommunicationService : Grape.Communication.CommunicationBase
    {
        private readonly List<CommunicationClient> communicationClients = new List<CommunicationClient>();
        private readonly IPAddress IPAddress;
        private readonly int Port;

        public CommunicationService(CommunicationServiceData data)
        {
            IPAddress = data.IPAdress;
            Port = data.Port;

            for (int i = 0; i < data.config.Count; i++)
            {
                if (data.config[i].Port != Port && data.config[i].IPAdress != IPAddress)
                {
                    GrpcChannel channel = GrpcChannel.ForAddress($"http://{data.config[i].IPAdress}:{data.config[i].Port}");
                    Grape.Communication.CommunicationClient client = new Grape.Communication.CommunicationClient(channel);

                    communicationClients.Add(new CommunicationClient(client, data.config[i].IPAdress, data.config[i].Port));
                }
            }
        }
        public override async Task SendPackage(IAsyncStreamReader<Grape.Package> requestStream, IServerStreamWriter<Grape.Package> responseStream, ServerCallContext context)
        {
            await foreach (Package package in requestStream.ReadAllAsync())
            {
                System.Console.WriteLine($"Command: {package.Command}");
            }
        }
    }
}
