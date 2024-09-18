using Grape;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrapeNetwork.Server.BuilderServer.Services
{
    public class CommunicationService : Communication.CommunicationBase
    {
        private readonly ILogger<CommunicationService> _logger;
       
        public List<Communication.CommunicationClient> communicationClients;

        public CommunicationService(ILogger<CommunicationService> logger)
        {
            _logger = logger;
        }
    }
}
