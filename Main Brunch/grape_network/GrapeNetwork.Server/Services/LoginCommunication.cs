using Grape;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static Grape.Login;

namespace GrapeNetwork.Server.BuilderServer.Services
{
    public class LoginCommunication : Login.LoginBase
    {
        private readonly ILogger<LoginCommunication> _logger;
        private Game.GameClient gameClient;
        private Database.DatabaseClient databaseClient;

        public LoginCommunication(ILogger<LoginCommunication> logger)
        {
            _logger = logger;
        }

        public override async Task GetAccountData(IAsyncStreamReader<RequestGetDataAccount> requestStream, IServerStreamWriter<ResponseGetDataAccount> responseStream, ServerCallContext context)
        {
            var readTask = Task.Run(async () =>
            {

            });

            await Task.WhenAll(readTask);
        }
    }
}
