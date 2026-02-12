using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using ReLoop.Application;
using ReLoop.Application.Abstractions;
using ReLoop.Application.Abstractions.Repositories;
using ReLoop.Infrastructure.Database;
using ReLoop.Infrastructure.Database.Repository;
using ReLoop.Infrastructure.Database.Services;
using ReLoop.Shared.Infrastructure.Services;

namespace ReLoop.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddHostedService<DatabaseInitializer>();
        services.AddMediatrWithFilters([typeof(Application.Extensions).Assembly]);

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IAiChatService, AiChatService>();

        // Add Semantic Kernel with Gemini
        var apiKey = configuration["llm:gemini:apiKey"]!;
        var model = configuration["llm:gemini:model"]!;

        var kernel = Kernel.CreateBuilder()
            .AddGoogleAIGeminiChatCompletion(model, apiKey)
            .Build();

        services.AddSingleton(kernel);
        services.AddSingleton(kernel.GetRequiredService<Microsoft.SemanticKernel.ChatCompletion.IChatCompletionService>());

        return services;
    }
}