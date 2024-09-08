using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grape;
using Grpc.Net.Client;

namespace GrapeNetwork.Server.BuilderServer
{
    public class DatabaseCommunication : Database.DatabaseBase
    {
        private readonly ILogger<DatabaseCommunication> logger;
        private Game.GameClient gameClient;
        private Login.LoginClient loginClient;

        public DatabaseCommunication(ILogger<DatabaseCommunication> logger)
        {
            this.logger = logger;          
        }

        public override async Task GetAccountData(IAsyncStreamReader<RequestGetDataAccount> requestStream, IServerStreamWriter<ResponseGetDataAccount> responseStream, ServerCallContext context)
        {
            Task readTask = Task.Run( async () =>
            {
                Console.WriteLine(1);
            });

            await Task.WhenAll(readTask);
        }
    }
}
