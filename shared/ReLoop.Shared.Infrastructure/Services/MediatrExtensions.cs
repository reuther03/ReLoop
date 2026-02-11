using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ReLoop.Shared.Abstractions;
using ReLoop.Shared.Abstractions.Kernel.Events;
using ReLoop.Shared.Abstractions.QueriesAndCommands.Commands;
using ReLoop.Shared.Abstractions.QueriesAndCommands.Queries;

namespace ReLoop.Shared.Infrastructure.Services;

public static class MediatrExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMediatrWithFilters(IEnumerable<Assembly> assemblies)
        {
            var handlerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    t is { IsAbstract: false, IsInterface: false } &&
                    t.GetInterfaces().Any(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(IDomainNotificationHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))) &&
                    !t.GetCustomAttributes<DecoratorAttribute>().Any());

            foreach (var handlerType in handlerTypes)
            {
                var interfaces = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(IDomainNotificationHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));


                foreach (var handlerInterface in interfaces)
                {
                    services.AddScoped(handlerInterface, handlerType);
                }
            }

            return services;
        }
    }
}