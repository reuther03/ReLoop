using Microsoft.EntityFrameworkCore;
using ReLoop.Api.Domain.User;
using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Shared.Abstractions.Kernel.ValueObjects.Ids;
using ReLoop.Shared.Infrastructure.Postgres;

namespace ReLoop.Infrastructure.Database.Repository;

internal class UserRepository : Repository<User, ReLoopDbContext>, IUserRepository
{
    private readonly ReLoopDbContext _context;

    public UserRepository(ReLoopDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Users.FindAsync([UserId.From(id)], cancellationToken);
}