using ReLoop.Shared.Abstractions.QueriesAndCommands.Requests;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Shared.Abstractions.Services;

public interface ISender
{
    Task<Result<TResponse>> Send<TResponse>(IRequest<Result<TResponse>> request, CancellationToken cancellationToken = default);
}