using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Exceptions;

namespace Trackly.Domain.UnitTests.Entities;

public class RefreshTokenTests
{
    private static readonly Guid ValidUserId = Guid.NewGuid();
    private const string ValidTokenHash = "a-hashed-token-value";

    // -------------------------------------------------------
    // RefreshToken.Create
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsActiveToken()
    {
        // Act
        var token = RefreshToken.Create(ValidUserId, ValidTokenHash, DateTime.UtcNow.AddDays(7));

        // Assert
        token.UserId.Should().Be(ValidUserId);
        token.TokenHash.Should().Be(ValidTokenHash);
        token.IsExpired.Should().BeFalse();
        token.IsRevoked.Should().BeFalse();
        token.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithExpiryInThePast_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => RefreshToken.Create(ValidUserId, ValidTokenHash, DateTime.UtcNow.AddDays(-1));

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyTokenHash_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => RefreshToken.Create(ValidUserId, "", DateTime.UtcNow.AddDays(7));

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
        var token = RefreshToken.Create(ValidUserId, ValidTokenHash, DateTime.UtcNow.AddMilliseconds(50));
        Thread.Sleep(100);

        // Assert
        token.IsExpired.Should().BeTrue();
        token.IsActive.Should().BeFalse();
    }

    // -------------------------------------------------------
    // Revoke
    // -------------------------------------------------------

    [Fact]
    public void Revoke_SetsRevokedAt()
    {
        // Arrange
        var token = RefreshToken.Create(ValidUserId, ValidTokenHash, DateTime.UtcNow.AddDays(7));

        // Act
        token.Revoke();

        // Assert
        token.IsRevoked.Should().BeTrue();
        token.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Revoke_WhenAlreadyRevoked_ThrowsInvalidRefreshTokenStateException()
    {
        // Arrange
        var token = RefreshToken.Create(ValidUserId, ValidTokenHash, DateTime.UtcNow.AddDays(7));
        token.Revoke();
        Action act = token.Revoke;

        // Assert
        act.Should().Throw<InvalidRefreshTokenStateException>();
    }

    // -------------------------------------------------------
    // RotateTo
    // -------------------------------------------------------

    [Fact]
    public void RotateTo_SetsReplacedByTokenIdAndRevokes()
    {
        // Arrange
        var token = RefreshToken.Create(ValidUserId, ValidTokenHash, DateTime.UtcNow.AddDays(7));
        var newTokenId = Guid.NewGuid();

        // Act
        token.RotateTo(newTokenId);

        // Assert
        token.ReplacedByTokenId.Should().Be(newTokenId);
        token.IsRevoked.Should().BeTrue();
    }

    [Fact]
    public void RotateTo_WhenAlreadyRevoked_ThrowsInvalidRefreshTokenStateException()
    {
        // Arrange
        var token = RefreshToken.Create(ValidUserId, ValidTokenHash, DateTime.UtcNow.AddDays(7));
        token.Revoke();
        Action act = () => token.RotateTo(Guid.NewGuid());

        // Assert
        act.Should().Throw<InvalidRefreshTokenStateException>();
    }
}
