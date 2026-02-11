using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReLoop.Shared.Abstractions.Services;

namespace ReLoop.Shared.Infrastructure.Services;

internal static class Extensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices(IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISender, Sender>();
            return services;
        }
    }
}