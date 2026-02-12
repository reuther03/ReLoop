using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using ReLoop.Client.Models;
using ReLoop.Shared.Contracts.Dto.Identity;
using ReLoop.Shared.Contracts.Result;

namespace ReLoop.Client.Services;

public class UserProfile
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public event Action? OnBalanceChanged;

    public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<Result<AccessToken>> LoginAsync(LoginModel model)
    {
        var response = await _http.PostAsJsonAsync("/sign-in", new
        {
            model.Email,
            model.Password
        });

        var result = await response.Content.ReadFromJsonAsync<Result<AccessToken>>();

        if (result is { IsSuccess: true, Value: not null })
        {
            await _localStorage.SetItemAsStringAsync("authToken", result.Value.Token);
            await _localStorage.SetItemAsStringAsync("userEmail", result.Value.Email);
            await _localStorage.SetItemAsStringAsync("userId", result.Value.UserId.ToString());
            ((ReLoopAuthStateProvider)_authStateProvider).NotifyAuthStateChanged();
        }

        return result!;
    }

    public async Task<Result<Guid>> RegisterAsync(RegisterModel model)
    {
        var response = await _http.PostAsJsonAsync("/sign-up", new
        {
            model.FirstName,
            model.LastName,
            model.Email,
            model.InputPassword
        });

        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();
        return result!;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("userEmail");
        await _localStorage.RemoveItemAsync("userId");
        ((ReLoopAuthStateProvider)_authStateProvider).NotifyAuthStateChanged();
    }

    public async Task<string?> GetTokenAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync("authToken");
        return token?.Trim('"');
    }

    public async Task<UserProfile?> GetProfileAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return null;

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var result = await _http.GetFromJsonAsync<Result<UserProfile>>("/me");
        return result?.Value;
    }

    public async Task<Result<decimal>?> AddBalanceAsync(decimal amount)
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return null;

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _http.PostAsync($"/me/balance?amount={amount.ToString(System.Globalization.CultureInfo.InvariantCulture)}", null);
        var result = await response.Content.ReadFromJsonAsync<Result<decimal>>();

        if (result is { IsSuccess: true })
            OnBalanceChanged?.Invoke();

        return result;
    }

    public void NotifyBalanceChanged()
    {
        OnBalanceChanged?.Invoke();
    }
}
