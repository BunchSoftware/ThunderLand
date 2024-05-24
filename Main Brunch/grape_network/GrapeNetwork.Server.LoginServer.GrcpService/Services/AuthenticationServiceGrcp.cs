using GrapeNetwork.Server.LoginServer.GrcpService;
using Grpc.Core;

namespace GrapeNetwork.Server.LoginServer.GrcpService.Services
{
    public class AuthenticationServiceGrcp : GrcpService.AuthenticationServiceGrcp.AuthenticationServiceGrcpBase
    {
        private readonly ILogger<AuthenticationServiceGrcp> _logger;
        public AuthenticationServiceGrcp(ILogger<AuthenticationServiceGrcp> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
