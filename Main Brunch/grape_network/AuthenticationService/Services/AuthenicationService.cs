using Grpc.Core;
using AuthenticationService;

namespace AuthenticationService.Services
{
    public class AuthenicationService : AuthenticationService.AuthenticationServiceBase
    {
        private readonly ILogger<AuthenicationService> _logger;
        public AuthenicationService(ILogger<AuthenicationService> logger)
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
