using Trackly.Domain.Common;
using Trackly.Domain.Exceptions;

namespace Trackly.Domain.Entities;

// A magic-link grant, not a session — validated fresh against TokenHash on
// every request (Api layer), never exchanged for a second credential.
// RevokedAt is the real kill switch; ExpiresAt is a generous backstop, not
// a short session clock, since this link is meant to be reused over the
// life of a contract. Deliberately no rotation (contrast with
// RefreshToken.RotateTo) — rotating a share link defeats its purpose.
public sealed class ClientRoomAccess : Entity
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsActive => !IsExpired && !IsRevoked;

    private ClientRoomAccess() { }

    public static ClientRoomAccess Create(Guid tenantId, Guid projectId, string tokenHash, DateTime expiresAt, Guid createdById)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException("TokenHash cannot be empty.", nameof(tokenHash));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("ExpiresAt must be in the future.", nameof(expiresAt));

        if (createdById == Guid.Empty)
            throw new ArgumentException("CreatedById cannot be empty.", nameof(createdById));

        return new ClientRoomAccess
        {
            TenantId = tenantId,
            ProjectId = projectId,
            TokenHash = tokenHash,
            ExpiresAt = expiresAt,
            CreatedById = createdById,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Revoke()
    {
        if (IsRevoked)
            throw new InvalidClientRoomAccessStateException("Client room access is already revoked.");

        RevokedAt = DateTime.UtcNow;
    }
}
