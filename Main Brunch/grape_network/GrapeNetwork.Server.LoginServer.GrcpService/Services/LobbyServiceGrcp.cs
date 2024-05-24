using GrapeNetwork.Server.LoginServer.GrcpService;
using Grpc.Core;

namespace GrapeNetwork.Server.LoginServer.GrcpService.Services
{
    public class LobbyServiceGrcp : GrcpService.LobbyServiceGrcp.LobbyServiceGrcpBase
    { 
        private readonly ILogger<LobbyServiceGrcp> _logger;
        public LobbyServiceGrcp(ILogger<LobbyServiceGrcp> logger)
        {
            _logger = logger;
        }
    }
}
