using Microsoft.EntityFrameworkCore;
using ReLoop.Application.Abstractions;

namespace ReLoop.Infrastructure.Database;

public class ReLoopDbContext : DbContext, IReLoopDbContext
{
    // public DbSet<User> Users => Set<User>();
    // public DbSet<CompetenceGroup> CompetencesGroups => Set<CompetenceGroup>();
    // public DbSet<Matching> Matchings => Set<Matching>();

    public ReLoopDbContext(DbContextOptions<ReLoopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}