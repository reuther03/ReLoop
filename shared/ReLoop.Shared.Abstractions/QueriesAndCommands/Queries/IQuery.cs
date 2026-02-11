using ReLoop.Shared.Abstractions.QueriesAndCommands.Requests;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Shared.Abstractions.QueriesAndCommands.Queries;

public interface IQueryBase;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IQueryBase;