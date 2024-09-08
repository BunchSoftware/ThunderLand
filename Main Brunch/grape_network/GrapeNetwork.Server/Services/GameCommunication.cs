using Grape;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.BuilderServer.Services
{
    public class GameCommunication : Game.GameBase
    {
        private readonly ILogger<GameCommunication> _logger;
        private Login.LoginClient loginClient;
        private Database.DatabaseClient databaseClient;

        public GameCommunication(ILogger<GameCommunication> logger)
        {
            _logger = logger;
        }

        public override async Task GetAccountData(IAsyncStreamReader<RequestGetDataAccount> requestStream, IServerStreamWriter<ResponseGetDataAccount> responseStream, ServerCallContext context)
        {
            var readTask = Task.Run(async () =>
            {
                Console.WriteLine(2);
            });

            await Task.WhenAll(readTask);
        }
    }
}
