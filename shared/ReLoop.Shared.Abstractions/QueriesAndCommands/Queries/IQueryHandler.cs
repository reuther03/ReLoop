using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Shared.Abstractions.QueriesAndCommands.Queries;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}