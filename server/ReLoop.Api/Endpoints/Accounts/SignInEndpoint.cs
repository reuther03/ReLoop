using ReLoop.Application.Features.Commands.SignInCommand;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;

namespace ReLoop.Api.Endpoints.Accounts;

internal sealed class SignInEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/sign-in",
                async (SignInCommand request, ISender sender) =>
                {
                    var result = await sender.Send(request);
                    return result;
                })
            .AllowAnonymous();
    }
}