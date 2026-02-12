using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace ReLoop.Shared.Abstractions.Api;

public abstract class EndpointBase
{
    public abstract void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder);

    public static void MapEndpoints(WebApplication app, Assembly assembly)
    {
        var endpointTypes = assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(EndpointBase)) && !type.IsAbstract)
            .Select(type => ActivatorUtilities.CreateInstance(app.Services, type) as EndpointBase)
            .Where(endpoint => endpoint is not null)
            .ToList();

        endpointTypes.ForEach(endpoint => endpoint!.AddEndpoint(app));
    }
}