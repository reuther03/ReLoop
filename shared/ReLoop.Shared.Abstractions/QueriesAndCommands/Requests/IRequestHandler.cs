using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Shared.Abstractions.QueriesAndCommands.Requests;

public interface IRequestHandler<in TCommand, TResult> where TCommand : IRequest<Result>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}