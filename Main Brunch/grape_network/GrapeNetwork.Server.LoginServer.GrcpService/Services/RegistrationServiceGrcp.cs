using GrapeNetwork.Server.LoginServer.GrcpService;
using Grpc.Core;

namespace GrapeNetwork.Server.LoginServer.GrcpService.Services
{
    public class RegistrationServiceGrcp : GrcpService.RegistrationServiceGrcp.RegistrationServiceGrcpBase
    {
        private readonly ILogger<RegistrationServiceGrcp> _logger;
        public RegistrationServiceGrcp(ILogger<RegistrationServiceGrcp> logger)
        {
            _logger = logger;
        }
    }
}
