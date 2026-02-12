using Microsoft.EntityFrameworkCore;
using ReLoop.Api.Domain.User;
using ReLoop.Application.Abstractions;

namespace ReLoop.Infrastructure.Database;

public class ReLoopDbContext : DbContext, IReLoopDbContext
{
    public DbSet<User> Users => Set<User>();


    public ReLoopDbContext(DbContextOptions<ReLoopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}