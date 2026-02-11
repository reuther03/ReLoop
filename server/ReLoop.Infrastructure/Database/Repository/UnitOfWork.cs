using ReLoop.Application.Abstractions;
using ReLoop.Shared.Infrastructure.Postgres;

namespace ReLoop.Infrastructure.Database.Repository;

public class UnitOfWork : BaseUnitOfWork<ReLoopDbContext>, IUnitOfWork
{
    public UnitOfWork(ReLoopDbContext context) : base(context)
    {
    }
}