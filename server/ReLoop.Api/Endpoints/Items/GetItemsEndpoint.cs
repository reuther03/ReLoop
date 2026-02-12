using ReLoop.Application.Features.Queries.GetItemsQuery;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;

namespace ReLoop.Api.Endpoints.Items;

internal sealed class GetItemsEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/items",
                async (ISender sender) =>
                {
                    var query = new GetItemsQuery();
                    var result = await sender.Send(query);
                    return result;
                })
            .AllowAnonymous();
    }
}
