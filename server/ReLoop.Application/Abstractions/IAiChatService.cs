using ReLoop.Application.Features.Dtos;

namespace ReLoop.Application.Abstractions;

public interface IAiChatService
{
    Task<AnalyzedImageTagResponse> GenerateTag(
        string? title,
        string? description,
        byte[] imageData,
        CancellationToken cancellationToken = default);
}