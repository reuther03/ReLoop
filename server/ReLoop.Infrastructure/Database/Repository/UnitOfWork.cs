using ReLoop.Application.Abstractions;
using ReLoop.Shared.Abstractions.Kernel.Database;
using ReLoop.Shared.Contracts.Result;
using ReLoop.Shared.Infrastructure.Postgres;

namespace ReLoop.Infrastructure.Database.Repository;

public class UnitOfWork : BaseUnitOfWork<ReLoopDbContext>, IUnitOfWork
{
    public UnitOfWork(ReLoopDbContext context) : base(context)
    {
    }
}