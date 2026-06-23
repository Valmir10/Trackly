using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class UserTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private const string ValidEmail = "john@acme.com";
    private const string ValidFirstName = "John";
    private const string ValidLastName = "Smith";

    // -------------------------------------------------------
    // User.Create — happy path
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsUserWithCorrectProperties()
    {
        // Arrange + Act
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Assert
        user.Id.Should().NotBe(Guid.Empty);
        user.TenantId.Should().Be(ValidTenantId);
        user.Email.Should().Be(ValidEmail);
        user.FirstName.Should().Be(ValidFirstName);
        user.LastName.Should().Be(ValidLastName);
        user.Role.Should().Be(UserRole.Member);
        user.IsEmailVerified.Should().BeFalse();
        user.LastLoginAt.Should().BeNull();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_NormalizesEmailToLowercase()
    {
        // Arrange + Act
        var user = User.Create(ValidTenantId, "JOHN@ACME.COM", ValidFirstName, ValidLastName);

        // Assert
        user.Email.Should().Be("john@acme.com");
    }

    [Fact]
    public void Create_TrimsEmailWhitespace()
    {
        // Arrange + Act
        var user = User.Create(ValidTenantId, "  john@acme.com  ", ValidFirstName, ValidLastName);

        // Assert
        user.Email.Should().Be("john@acme.com");
    }

    [Fact]
    public void Create_DefaultsToMemberRole()
    {
        // Arrange + Act
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Assert
        user.Role.Should().Be(UserRole.Member);
    }

    // -------------------------------------------------------
    // User.Create — domänhändelse
    // -------------------------------------------------------

    [Fact]
    public void Create_RaisesUserCreatedEvent()
    {
        // Arrange + Act
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Assert
        user.DomainEvents.Should().ContainSingle(e => e is UserCreatedEvent);

        var domainEvent = user.DomainEvents.Single() as UserCreatedEvent;
        domainEvent!.UserId.Should().Be(user.Id);
        domainEvent.TenantId.Should().Be(ValidTenantId);
        domainEvent.Email.Should().Be(ValidEmail);
    }

    // -------------------------------------------------------
    // User.Create — ogiltiga indata
    // -------------------------------------------------------

    [Fact]
    public void Create_WithEmptyGuidTenantId_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => User.Create(Guid.Empty, ValidEmail, ValidFirstName, ValidLastName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .Which.Message.Should().Contain("TenantId");
    }

    [Fact]
    public void Create_WithEmptyEmail_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => User.Create(ValidTenantId, "", ValidFirstName, ValidLastName);

        // Assert
        act.Should().Throw<ArgumentException>()
            .Which.Message.Should().Contain("Email");
    }

    [Fact]
    public void Create_WithEmptyFirstName_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => User.Create(ValidTenantId, ValidEmail, "", ValidLastName);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyLastName_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => User.Create(ValidTenantId, ValidEmail, ValidFirstName, "");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // FullName
    // -------------------------------------------------------

    [Fact]
    public void FullName_ReturnsCombinedFirstAndLastName()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Assert
        user.FullName.Should().Be("John Smith");
    }

    // -------------------------------------------------------
    // SetPasswordHash
    // -------------------------------------------------------

    [Fact]
    public void SetPasswordHash_WithValidHash_StoresHash()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        var hash = "$2a$12$somehashedpassword";

        // Act
        user.SetPasswordHash(hash);

        // Assert
        user.PasswordHash.Should().Be(hash);
    }

    [Fact]
    public void SetPasswordHash_WithEmptyHash_ThrowsArgumentException()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        Action act = () => user.SetPasswordHash("");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // VerifyEmail
    // -------------------------------------------------------

    [Fact]
    public void VerifyEmail_SetsIsEmailVerifiedToTrue()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Act
        user.VerifyEmail();

        // Assert
        user.IsEmailVerified.Should().BeTrue();
    }

    [Fact]
    public void VerifyEmail_ClearsEmailVerificationToken()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        user.SetEmailVerificationToken("some-token-123");

        // Act
        user.VerifyEmail();

        // Assert
        user.EmailVerificationToken.Should().BeNull();
    }

    // -------------------------------------------------------
    // Password reset token
    // -------------------------------------------------------

    [Fact]
    public void IsPasswordResetTokenValid_WithCorrectTokenAndFutureExpiry_ReturnsTrue()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        user.SetPasswordResetToken("reset-token-abc", DateTime.UtcNow.AddHours(1));

        // Act
        var result = user.IsPasswordResetTokenValid("reset-token-abc");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsPasswordResetTokenValid_WithExpiredToken_ReturnsFalse()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        user.SetPasswordResetToken("reset-token-abc", DateTime.UtcNow.AddMinutes(-1));

        // Act
        var result = user.IsPasswordResetTokenValid("reset-token-abc");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsPasswordResetTokenValid_WithWrongToken_ReturnsFalse()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        user.SetPasswordResetToken("correct-token", DateTime.UtcNow.AddHours(1));

        // Act
        var result = user.IsPasswordResetTokenValid("wrong-token");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ClearPasswordResetToken_SetsTokenAndExpiryToNull()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        user.SetPasswordResetToken("some-token", DateTime.UtcNow.AddHours(1));

        // Act
        user.ClearPasswordResetToken();

        // Assert
        user.PasswordResetToken.Should().BeNull();
        user.PasswordResetTokenExpiry.Should().BeNull();
    }

    // -------------------------------------------------------
    // UpdateLastLogin
    // -------------------------------------------------------

    [Fact]
    public void UpdateLastLogin_SetsLastLoginAtToCurrentTime()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Act
        user.UpdateLastLogin();

        // Assert
        user.LastLoginAt.Should().NotBeNull();
        user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    // -------------------------------------------------------
    // UpdateProfile
    // -------------------------------------------------------

    [Fact]
    public void UpdateProfile_WithValidNames_UpdatesFirstAndLastName()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Act
        user.UpdateProfile("Jane", "Doe");

        // Assert
        user.FirstName.Should().Be("Jane");
        user.LastName.Should().Be("Doe");
    }

    [Fact]
    public void UpdateProfile_WithEmptyFirstName_ThrowsArgumentException()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);
        Action act = () => user.UpdateProfile("", "Doe");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // SetRole
    // -------------------------------------------------------

    [Fact]
    public void SetRole_UpdatesUserRole()
    {
        // Arrange
        var user = User.Create(ValidTenantId, ValidEmail, ValidFirstName, ValidLastName);

        // Act
        user.SetRole(UserRole.Admin);

        // Assert
        user.Role.Should().Be(UserRole.Admin);
    }
}
