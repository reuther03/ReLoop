using System.Net.Http.Headers;
using System.Net.Http.Json;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Client.Services;

public record ItemDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    string Status,
    Guid SellerId,
    string SellerName,
    DateTime CreatedAt
);

public class ItemService
{
    private readonly HttpClient _http;
    private readonly AuthService _authService;

    public ItemService(HttpClient http, AuthService authService)
    {
        _http = http;
        _authService = authService;
    }

    public async Task<List<ItemDto>> GetItemsAsync()
    {
        await SetAuthHeader();
        var result = await _http.GetFromJsonAsync<Result<IEnumerable<ItemDto>>>("/items");
        return result?.Value?.ToList() ?? new List<ItemDto>();
    }

    public async Task<Result<Guid>?> CreateItemAsync(string name, string description, decimal price, Stream imageStream, string fileName)
    {
        await SetAuthHeader();

        var encodedName = Uri.EscapeDataString(name);
        var encodedDesc = Uri.EscapeDataString(description);
        var priceStr = price.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var url = $"/items?name={encodedName}&description={encodedDesc}&price={priceStr}";

        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(imageStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(streamContent, "image", fileName);

        var response = await _http.PostAsync(url, content);
        return await response.Content.ReadFromJsonAsync<Result<Guid>>();
    }

    public async Task<List<ItemDto>> GetUserItemsAsync()
    {
        await SetAuthHeader();
        var result = await _http.GetFromJsonAsync<Result<IEnumerable<ItemDto>>>("/items/my");
        return result?.Value?.ToList() ?? new List<ItemDto>();
    }

    public async Task<Result<Guid>?> BuyItemAsync(Guid itemId)
    {
        await SetAuthHeader();
        var response = await _http.PostAsync($"/items/{itemId}/buy", null);
        return await response.Content.ReadFromJsonAsync<Result<Guid>>();
    }

    public string GetImageUrl(Guid itemId)
    {
        return $"{_http.BaseAddress}items/{itemId}/image";
    }

    private async Task SetAuthHeader()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
