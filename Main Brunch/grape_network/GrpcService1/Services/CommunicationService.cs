using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using Grpc.Core;
using Grape;
using GrapeNetwork.Server.Core.Configuration;
using Grpc.Net.Client;
using GrapeNetwork.Server.BuilderServer;
using System.Net;
using GrpcService1.Services;


    public class CommunicationService : Communication.CommunicationBase
    {
        private readonly List<Communication.CommunicationClient> communicationClients = new List<Communication.CommunicationClient>();

        private readonly ILogger<Communication.CommunicationBase> _logger;
        public CommunicationService(ILogger<Communication.CommunicationBase> logger)
        {
            _logger = logger;
        }

    private readonly IPAddress IPAddress;
    private readonly int Port;

    public CommunicationService(CommunicationServiceData data)
    {
        //IPAddress = data.IPAdress;
        //Port = data.Port;

        //for (int i = 0; i < data.configCommunicationServices.Count; i++)
        //{
        //    if (data.configCommunicationServices[i].Port != Port && data.configCommunicationServices[i].IPAdress != IPAddress)
        //    {
        //        GrpcChannel channel = GrpcChannel.ForAddress($"http://{data.configCommunicationServices[i].IPAdress}:{data.configCommunicationServices[i].Port}");
        //        Grape.Communication.CommunicationClient client = new Grape.Communication.CommunicationClient(channel);

        //        var call = client.SendPackage();
        //        call.RequestStream.WriteAsync(new Package { Command = 1 });
        //        call.RequestStream.CompleteAsync();

        //        communicationClients.Add(client);
        //    }
        //}
    }

    public override async Task SendPackage(IAsyncStreamReader<Package> requestStream, IServerStreamWriter<Package> responseStream, ServerCallContext context)
    {
        //var readTask = Task.Run(async () =>
        //{
        //    await foreach (Package message in requestStream.ReadAllAsync())
        //    {
        //        Console.WriteLine($"Command: {message.Command}");
        //    }
        //});
        //for (int i = 0; i < communicationClients.Count; i++)
        //{
        //    var call = communicationClients[i].SendPackage();
        //    await call.RequestStream.WriteAsync(new Package { Command = 1 });
        //    await call.RequestStream.CompleteAsync();
        //}
        var a = Task.Run(async () =>
        {
            await responseStream.WriteAsync(new Package { Command = 1 });
        });
        await a;
    }
    }
