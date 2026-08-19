using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Exceptions;

namespace Trackly.Domain.UnitTests.Entities;

public class ClientRoomAccessTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidCreatedById = Guid.NewGuid();
    private const string ValidTokenHash = "a-hashed-token-value";

    // -------------------------------------------------------
    // ClientRoomAccess.Create
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsActiveAccess()
    {
        // Act
        var access = ClientRoomAccess.Create(ValidTenantId, ValidProjectId, ValidTokenHash, DateTime.UtcNow.AddDays(365), ValidCreatedById);

        // Assert
        access.TenantId.Should().Be(ValidTenantId);
        access.ProjectId.Should().Be(ValidProjectId);
        access.TokenHash.Should().Be(ValidTokenHash);
        access.IsExpired.Should().BeFalse();
        access.IsRevoked.Should().BeFalse();
        access.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithExpiryInThePast_ThrowsArgumentException()
    {
        // Act
        var act = () => ClientRoomAccess.Create(ValidTenantId, ValidProjectId, ValidTokenHash, DateTime.UtcNow.AddDays(-1), ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyTokenHash_ThrowsArgumentException()
    {
        // Act
        var act = () => ClientRoomAccess.Create(ValidTenantId, ValidProjectId, "", DateTime.UtcNow.AddDays(365), ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // IsActive
    // -------------------------------------------------------

    [Fact]
    public void IsActive_WhenExpiryIsInThePast_IsFalse()
    {
        // Arrange — Create() rejects a past expiry directly, so build one that expires immediately instead.
        var access = ClientRoomAccess.Create(ValidTenantId, ValidProjectId, ValidTokenHash, DateTime.UtcNow.AddMilliseconds(50), ValidCreatedById);
        Thread.Sleep(100);

        // Assert
        access.IsExpired.Should().BeTrue();
        access.IsActive.Should().BeFalse();
    }

    // -------------------------------------------------------
    // Revoke
    // -------------------------------------------------------

    [Fact]
    public void Revoke_SetsRevokedAt()
    {
        // Arrange
        var access = ClientRoomAccess.Create(ValidTenantId, ValidProjectId, ValidTokenHash, DateTime.UtcNow.AddDays(365), ValidCreatedById);

        // Act
        access.Revoke();

        // Assert
        access.IsRevoked.Should().BeTrue();
        access.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Revoke_WhenAlreadyRevoked_ThrowsInvalidClientRoomAccessStateException()
    {
        // Arrange
        var access = ClientRoomAccess.Create(ValidTenantId, ValidProjectId, ValidTokenHash, DateTime.UtcNow.AddDays(365), ValidCreatedById);
        access.Revoke();
        var act = access.Revoke;

        // Assert
        act.Should().Throw<InvalidClientRoomAccessStateException>();
    }
}
