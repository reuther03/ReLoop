using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReLoop.Infrastructure.Database;

namespace ReLoop.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddHostedService<DatabaseInitializer>();

        return services;
    }
}