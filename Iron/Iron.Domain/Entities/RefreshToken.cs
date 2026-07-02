namespace Iron.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public long UserId { get; private set; }

    public string Token { get; private set; } = string.Empty;

    public DateTime ExpiresAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public User User { get; private set; } = null!;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsRevoked => RevokedAt.HasValue;

    public bool IsActive => !IsExpired && !IsRevoked;

    private RefreshToken() { }

    private RefreshToken(long userId, string token, DateTime expiresAt)
    {
        CreatedAt = DateTime.UtcNow;
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Create(long userId, string token, DateTime expiresAt)
    {
        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token não pode ser vazio.", nameof(token));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentOutOfRangeException(nameof(expiresAt), "Data de expiração deve ser futura.");

        return new RefreshToken(userId, token, expiresAt);
    }

    public void Revoke()
    {
        if (IsRevoked)
            throw new InvalidOperationException("Refresh token já foi revogado.");

        RevokedAt = DateTime.UtcNow;
    }
}
