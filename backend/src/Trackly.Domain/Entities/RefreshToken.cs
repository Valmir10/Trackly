using Trackly.Domain.Common;
using Trackly.Domain.Exceptions;

namespace Trackly.Domain.Entities;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsExpired && !IsRevoked;

    private RefreshToken() { }

    public static RefreshToken Create(Guid userId, string tokenHash, DateTime expiresAt)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("TokenHash cannot be empty.", nameof(tokenHash));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("ExpiresAt must be in the future.", nameof(expiresAt));

        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Revoke()
    {
        if (IsRevoked)
            throw new InvalidRefreshTokenStateException("Refresh token is already revoked.");

        RevokedAt = DateTime.UtcNow;
    }

    public void RotateTo(Guid newTokenId)
    {
        if (!IsActive)
            throw new InvalidRefreshTokenStateException("Cannot rotate a refresh token that is not active.");

        ReplacedByTokenId = newTokenId;
        RevokedAt = DateTime.UtcNow;
    }
}
