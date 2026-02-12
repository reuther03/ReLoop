using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.Api;

namespace ReLoop.Api.Endpoints.Items;

internal sealed class GetItemImageEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/items/{itemId:guid}/image",
                async (Guid itemId, IItemRepository itemRepository) =>
                {
                    var item = await itemRepository.GetByIdAsync(itemId);
                    if (item is null)
                        return Results.NotFound();

                    return Results.File(item.ImageData, "image/jpeg");
                })
            .AllowAnonymous();
    }
}
