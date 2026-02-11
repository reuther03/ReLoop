using Microsoft.Extensions.DependencyInjection;

namespace ReLoop.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly));
        // services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Extensions).Assembly));
        // services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        return services;
    }
}