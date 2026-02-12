using ReLoop.Application.Features.Queries.GetUserItemsQuery;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;

namespace ReLoop.Api.Endpoints.Items;

internal sealed class GetUserItemsEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/items/my",
                async (ISender sender) =>
                {
                    var query = new GetUserItems();
                    var result = await sender.Send(query);
                    return result;
                })
            .RequireAuthorization();
    }
}
