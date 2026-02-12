using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Api.Endpoints.Accounts;

internal sealed class GetMeEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGet("/me",
                async (IUserService userService, IUserRepository userRepository) =>
                {
                    if (!userService.IsAuthenticated)
                        return Results.Json(Result.Unauthorized("Not authenticated"));

                    var user = await userRepository.GetByIdAsync(userService.UserId.Value);
                    if (user is null)
                        return Results.Json(Result.NotFound("User not found"));

                    var response = Result.Ok(new
                    {
                        Id = user.Id.Value,
                        Email = user.Email.Value,
                        FirstName = user.FirstName.Value,
                        LastName = user.LastName.Value,
                        Balance = user.Balance
                    });

                    return Results.Json(response);
                })
            .RequireAuthorization();
    }
}
