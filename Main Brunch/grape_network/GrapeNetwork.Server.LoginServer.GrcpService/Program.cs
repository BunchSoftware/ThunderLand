using GrapeNetwork.Server.LoginServer.GrcpService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
var app = builder.Build();
app.MapGrpcService<AuthenticationServiceGrcp>();
app.MapGrpcService<RegistrationServiceGrcp>();
app.MapGrpcService<LobbyServiceGrcp>();
app.MapGet("/", () => "Hello World !");
app.Run();

