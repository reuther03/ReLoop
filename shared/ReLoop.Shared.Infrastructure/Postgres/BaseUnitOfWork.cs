using Microsoft.EntityFrameworkCore;
using ReLoop.Shared.Abstractions.Kernel.Database;
using ReLoop.Shared.Abstractions.Kernel.Primitives;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Shared.Infrastructure.Postgres;

public abstract class BaseUnitOfWork<T> : IBaseUnitOfWork where T : DbContext
{
    private readonly T _context;

    protected BaseUnitOfWork(T context)
    {
        _context = context;
    }

    public virtual async Task<Result> CommitAsync(CancellationToken cancellationToken = default)
    {
        bool commitStatus;

        try
        {
            var changes =  await _context.SaveChangesAsync(cancellationToken);
            if (changes <= 0)
                return Result.InternalServerError("An error occurred while saving changes to the database.");

            commitStatus = true;
        }
        catch (Exception)
        {
            commitStatus = false;
        }

        return commitStatus
            ? Result.Ok()
            : Result.InternalServerError("An error occurred while saving changes to the database.");
    }
}