using Grpc.Core;
using AuthenticationService;

namespace GrapeNetwork.AuthenticationServiceGrcp.Services
{
    public class AuthenticationServiceGrcp : AuthenticationService.AuthenticationServiceGrcp.AuthenticationServiceGrcpBase
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
