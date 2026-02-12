using System.Reflection;
using ReLoop.Api.Domain;
using ReLoop.Application;
using ReLoop.Infrastructure;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5151", "https://localhost:7161")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

services.AddDomain()
    .AddApplication()
    .AddInfrastructure(configuration)
    .AddSharedInfrastructure(configuration);

var app = builder.Build();

app.UseCors();
app.UseInfrastructure();

EndpointBase.MapEndpoints(app, Assembly.GetExecutingAssembly());

await app.RunAsync();