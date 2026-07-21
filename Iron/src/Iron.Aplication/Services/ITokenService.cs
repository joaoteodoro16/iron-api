using Iron.Domain.Entities;

namespace Iron.Aplication.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
    public (string RefreshToken, DateTime RefreshExpiresAt) GenerateRefreshToken(User user);

    public Task<(bool IsValid, long UserId)> ValidateToken(string token);
}
