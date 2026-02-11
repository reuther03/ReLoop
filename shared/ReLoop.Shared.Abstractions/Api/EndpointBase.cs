using Microsoft.AspNetCore.Routing;

namespace ReLoop.Shared.Abstractions.Api;

public abstract class EndpointBase
{
    public abstract void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}