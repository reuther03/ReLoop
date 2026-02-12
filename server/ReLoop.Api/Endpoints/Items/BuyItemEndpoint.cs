using ReLoop.Application.Features.Commands.SellItemCommand;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;

namespace ReLoop.Api.Endpoints.Items;

internal sealed class BuyItemEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/items/{itemId:guid}/buy",
                async (Guid itemId, ISender sender) =>
                {
                    var command = new SellItemCommand(itemId);
                    var result = await sender.Send(command);
                    return result;
                })
            .RequireAuthorization();
    }
}
