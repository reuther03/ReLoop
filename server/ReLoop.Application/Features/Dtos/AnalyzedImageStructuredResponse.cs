namespace ReLoop.Application.Features.Dtos;

public record AnalyzedImageTagResponse
{
    public string Tag { get; init; } = string.Empty;
}