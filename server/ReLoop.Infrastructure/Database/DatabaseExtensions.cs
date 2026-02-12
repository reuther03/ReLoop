using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReLoop.Application.Abstractions;
using ReLoop.Infrastructure.Database.Repository;
using ReLoop.Shared.Infrastructure.Postgres;

namespace ReLoop.Infrastructure.Database;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPostgres<ReLoopDbContext>(configuration)
            .AddScoped<IReLoopDbContext, ReLoopDbContext>()
            .AddUnitOfWork<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<DatabaseInitializer>();

        return services;
    }
}