namespace ReLoop.Shared.Contracts.Dto.Identity;

public class AccessToken
{
    public string Token { get; init; } = null!;
    public Guid UserId { get; init; }
    public string Email { get; init; } = null!;
    public string Role { get; init; } = null!;


    public static AccessToken Create(string token, Guid userId, string email, string role)
    {
        return new AccessToken
        {
            Token = token,
            UserId = userId,
            Email = email,
            Role = role
        };
    }
}