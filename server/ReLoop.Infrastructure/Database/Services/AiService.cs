using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using ReLoop.Application.Abstractions;
using ReLoop.Application.Features.Dtos;

namespace ReLoop.Infrastructure.Database.Services;

public class AiService : IAiService
{
    private readonly IChatCompletionService _chatCompletionService;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public AiService(IChatCompletionService chatCompletionService)
    {
        _chatCompletionService = chatCompletionService;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }


    public async Task<AnalyzedImageTagResponse> GenerateTag(string? title, string? description, byte[] imageData, CancellationToken cancellationToken = default)
    {
        var prompt =
            $$"""
              You are an item categorization assistant for a marketplace app.
              Analyze the provided item image and classify it into exactly ONE category.

              Item title: {{title ?? "Not provided"}}
              Item description: {{description ?? "Not provided"}}

              Available categories:
              - Other (use only if no other category fits)
              - Clothes (clothing, shoes, accessories, fashion items)
              - Electronics (phones, computers, TVs, gadgets, cables, chargers)
              - Books (books, magazines, comics, textbooks)
              - Sports (sports equipment, fitness gear, outdoor activities)
              - HomeAndGarden (home decor, kitchen items, garden tools, plants)
              - Toys (children's toys, board games, puzzles)
              - Music (instruments, vinyl records, CDs, music equipment)
              - Movies (DVDs, Blu-rays, movie memorabilia)
              - Games (video games, consoles, gaming accessories)
              - Furniture (chairs, tables, beds, sofas, shelves)
              - Automotive (car parts, accessories, tools for vehicles)
              - Beauty (cosmetics, skincare, perfumes, hair products)
              - Jewelry (rings, necklaces, watches, bracelets)
              - Collectibles (antiques, rare items, trading cards, stamps, coins)

              Respond with JSON containing single field "tag" with the category name.
              Example response: {"tag":"Electronics"}
              """;


        var systemMessage = new ChatMessageContent(AuthorRole.System, prompt);

        var chatHistory = new ChatHistory();

        var complexUserMessage = new ChatMessageContent(AuthorRole.User, new ChatMessageContentItemCollection
        {
            new ImageContent(imageData, "image/jpeg")
        });

        chatHistory.AddRange([systemMessage, complexUserMessage]);

        var response = await _chatCompletionService.GetChatMessageContentsAsync(chatHistory, new GeminiPromptExecutionSettings
        {
            MaxTokens = 2000,
            Temperature = 0.3f,
            ThinkingConfig = new GeminiThinkingConfig { ThinkingBudget = 0 },
            ResponseMimeType = "application/json"
        }, cancellationToken: cancellationToken);

        var responseText = response[0].Content;

        if (string.IsNullOrWhiteSpace(responseText))
            throw new InvalidOperationException("Vision model returned empty response");

        var structuredResponse = JsonSerializer.Deserialize<AnalyzedImageTagResponse>(responseText, _jsonSerializerOptions)!;

        return structuredResponse;
    }
}