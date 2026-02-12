using ReLoop.Application.Abstractions;
using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.Api;
using ReLoop.Shared.Abstractions.Services;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Api.Endpoints.Accounts;

internal sealed class AddBalanceEndpoint : EndpointBase
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("/me/balance",
                async (decimal amount, IUserService userService, IUserRepository userRepository, IUnitOfWork unitOfWork) =>
                {
                    if (!userService.IsAuthenticated)
                        return Results.Json(Result.Unauthorized("Not authenticated"));

                    if (amount <= 0)
                        return Results.Json(Result.BadRequest("Amount must be greater than 0"));

                    var user = await userRepository.GetByIdAsync(userService.UserId.Value);
                    if (user is null)
                        return Results.Json(Result.NotFound("User not found"));

                    user.UpdateBalance(amount);
                    await unitOfWork.CommitAsync();

                    return Results.Json(Result.Ok(user.Balance));
                })
            .RequireAuthorization();
    }
}
