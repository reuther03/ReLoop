using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReLoop.Shared.Abstractions.Kernel.Database;

namespace ReLoop.Shared.Infrastructure.Postgres;

public static class Extensions
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddPostgres()
        {
            var options = services.GetOptions<PostgresOptions>("postgres");
            services.AddSingleton(options);
            services.AddSingleton(UnitOfWorkTypeRegistry.Instance);

            return services;
        }

        // public IServiceCollection AddDecorators()
        // {
        //     services.TryDecorate(typeof(ICommandHandler<,>), typeof(TransactionalCommandHandlerDecorator<>));
        //     services.TryDecorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        //     services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.BaseCommandHandler<>));
        //     services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));
        //
        //
        //     return services;
        // }

        public IServiceCollection AddPostgres<T>(IConfiguration configuration) where T : DbContext
        {
            var options = configuration.GetOptions<PostgresOptions>("postgres");
            services.AddDbContext<T>(x => x.UseNpgsql(options.ConnectionString).UseNpgsql(options.ConnectionString));
            return services;
        }

        // public IServiceCollection AddRedis()
        // {
        //     services.AddStackExchangeRedisCache(options =>
        //     {
        //         var redisOptions = services.GetOptions<RedisOptions>("redis");
        //         options.Configuration = redisOptions.ConnectionString;
        //     });
        //
        //     return services;
        // }

        public IServiceCollection AddUnitOfWork<TUnitOfWork, TImplementation>()
            where TUnitOfWork : class, IBaseUnitOfWork where TImplementation : class, TUnitOfWork
        {
            services.AddScoped<TUnitOfWork, TImplementation>();
            services.AddScoped<IBaseUnitOfWork, TImplementation>();

            UnitOfWorkTypeRegistry.Instance.Register<TUnitOfWork>();

            return services;
        }
    }
}