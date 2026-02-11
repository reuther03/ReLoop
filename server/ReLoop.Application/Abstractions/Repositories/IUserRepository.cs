using ReLoop.Api.Domain.User;
using ReLoop.Shared.Abstractions.Kernel.Database;

namespace ReLoop.Application.Abstractions.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<bool> ExistsWithEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}