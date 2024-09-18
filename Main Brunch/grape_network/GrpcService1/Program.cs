using GrapeNetwork.Server.BuilderServer;
using GrpcService1;
using GrpcService1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton(new
     CommunicationService(new CommunicationServiceData()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CommunicationService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
