using ReLoop.Shared.Abstractions.QueriesAndCommands.Requests;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Shared.Abstractions.QueriesAndCommands.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;