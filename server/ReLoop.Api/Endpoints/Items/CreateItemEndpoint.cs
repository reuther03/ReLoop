using ReLoop.Application.Features.Commands.CreateItemCommand;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;

namespace ReLoop.Api.Endpoints.Items;

internal sealed class CreateItemEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/items",
                async (IFormFile image, string name, string description, decimal price, ISender sender) =>
                {
                    var command = new CreateItemCommand(name, description, price, image);
                    var result = await sender.Send(command);
                    return result;
                })
            .DisableAntiforgery()
            .RequireAuthorization();
    }
}
