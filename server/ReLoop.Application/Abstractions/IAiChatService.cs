using ReLoop.Application.Features.Dtos;

namespace ReLoop.Application.Abstractions;

public interface IAiService
{
    Task<AnalyzedImageTagResponse> GenerateTag(
        string? title,
        string? description,
        byte[] imageData,
        CancellationToken cancellationToken = default);
}