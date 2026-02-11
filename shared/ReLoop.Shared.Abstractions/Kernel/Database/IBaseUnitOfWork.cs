using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Shared.Abstractions.Kernel.Database;

public interface IBaseUnitOfWork
{
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}