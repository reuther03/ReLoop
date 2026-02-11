using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReLoop.Shared.Infrastructure.Auth;
using ReLoop.Shared.Infrastructure.Postgres;
using ReLoop.Shared.Infrastructure.Services;
using ReLoop.Shared.Infrastructure.Swagger;

[assembly: InternalsVisibleTo("ReLoop.Api")]

namespace ReLoop.Shared.Infrastructure;

internal static class Extensions
{
    private const string CorsPolicy = "cors";

    extension(IServiceCollection services)
    {
        public IServiceCollection AddSharedInfrastructure(IConfiguration configuration)
        {
            services.AddCors(cors =>
            {
                cors.AddPolicy(CorsPolicy, x =>
                {
                    x.WithOrigins()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.ConfigureHttpJsonOptions(opt => { opt.SerializerOptions.PropertyNameCaseInsensitive = true; });
            services.AddSwagger();
            services.AddAuth(configuration);
            services.AddHostedService<AppInitializer>();
            services.AddServices(configuration);
            services.AddPostgres();
            services.AddMediatrWithFilters([Assembly.GetExecutingAssembly()]);

            return services;
        }
    }

    extension(IApplicationBuilder app)
    {
        public IApplicationBuilder UseInfrastructure()
        {
            app.UseRouting();
            app.UseCors(CorsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReLoop API"); });
            app.UseAuthorization();
            return app;
        }
    }

    extension(IServiceCollection services)
    {
        public T GetOptions<T>(string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration.GetOptions<T>(sectionName);
        }
    }

    extension(IConfiguration configuration)
    {
        public T GetOptions<T>(string sectionName) where T : new()
        {
            var options = new T();
            configuration.GetSection(sectionName).Bind(options);
            return options;
        }
    }

    public static string GetModuleName(this object value)
        => value?.GetType().GetModuleName() ?? string.Empty;

    public static string GetModuleName(this Type type)
    {
        if (type?.Namespace is null)
        {
            return string.Empty;
        }

        return type.Namespace.StartsWith("TaskManager.Modules.")
            ? type.Namespace.Split('.')[2].ToLowerInvariant()
            : string.Empty;
    }
}