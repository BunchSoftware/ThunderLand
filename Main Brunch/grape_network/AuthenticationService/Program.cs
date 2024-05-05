using AuthenticationService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthenicationService>();
app.MapGet("/", () => "");
app.Run();
