using Microsoft.EntityFrameworkCore;
using ReLoop.Api.Domain.User;

namespace ReLoop.Application.Abstractions;

public interface IReLoopDbContext
{
    DbSet<User> Users { get; }

}