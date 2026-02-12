namespace ReLoop.Application.Features.Dtos;

public record ItemDto(
    Guid Id,
    string Name,
    string Description,
    byte[] ImageData,
    decimal Price,
    string Category,
    string Status,
    Guid SellerId,
    string SellerName,
    DateTime CreatedAt
);
