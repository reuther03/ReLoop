using ReLoop.Api.Domain;
using ReLoop.Application;
using ReLoop.Infrastructure;
using ReLoop.Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddDomain()
    .AddApplication()
    .AddInfrastructure()
    .AddSharedInfrastructure(configuration);


services.AddEndpointsApiExplorer();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseInfrastructure();


await app.RunAsync();