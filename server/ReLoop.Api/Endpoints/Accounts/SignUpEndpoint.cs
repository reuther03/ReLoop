using ReLoop.Application.Features.Commands.SignUpCommand;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;

namespace ReLoop.Api.Endpoints.Accounts;

internal sealed class SignUpEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/sign-up",
                async (SignUpCommand request, ISender sender) =>
                {
                    var result = await sender.Send(request);
                    return result;
                })
            .AllowAnonymous();
    }
}