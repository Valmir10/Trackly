using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Auth.Commands.Login;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Auth.Commands.Login;

public class LoginCommandHandlerTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IJwtTokenService _jwtTokenService = Substitute.For<IJwtTokenService>();

    private LoginCommandHandler CreateHandler() => new(
        _tenantRepository, _userRepository, _refreshTokenRepository, _unitOfWork, _passwordHasher, _jwtTokenService);

    private static User ExistingUser(Guid tenantId, string passwordHash)
    {
        var user = User.Create(tenantId, "john@acme.com", "John", "Smith");
        user.SetPasswordHash(passwordHash);
        return user;
    }

    // -------------------------------------------------------
    // Handle — happy path
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WithValidCredentials_ReturnsAuthResult()
    {
        // Arrange
        var tenant = Tenant.Create("Acme Corp", "acme-corp");
        var user = ExistingUser(tenant.Id, "hashed-password");
        _tenantRepository.GetBySlugAsync("acme-corp", Arg.Any<CancellationToken>()).Returns(tenant);
        _userRepository.GetByEmailAsync(tenant.Id, "john@acme.com", Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify("correct-password", "hashed-password").Returns(true);
        _jwtTokenService.GenerateAccessToken(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>()).Returns("access-token");
        _jwtTokenService.GenerateRefreshToken().Returns(("raw-refresh-token", "hashed-refresh-token"));
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new LoginCommand("acme-corp", "john@acme.com", "correct-password"), CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be("access-token");
        result.UserId.Should().Be(user.Id);
        result.TenantId.Should().Be(tenant.Id);
    }

    [Fact]
    public async Task Handle_UpdatesLastLogin()
    {
        // Arrange
        var tenant = Tenant.Create("Acme Corp", "acme-corp");
        var user = ExistingUser(tenant.Id, "hashed-password");
        _tenantRepository.GetBySlugAsync("acme-corp", Arg.Any<CancellationToken>()).Returns(tenant);
        _userRepository.GetByEmailAsync(tenant.Id, "john@acme.com", Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
        _jwtTokenService.GenerateRefreshToken().Returns(("raw-refresh-token", "hashed-refresh-token"));
        var handler = CreateHandler();

        // Act
        await handler.Handle(new LoginCommand("acme-corp", "john@acme.com", "correct-password"), CancellationToken.None);

        // Assert
        user.LastLoginAt.Should().NotBeNull();
    }

    // -------------------------------------------------------
    // Handle — invalid credentials (same message regardless of cause)
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsUnauthorizedException()
    {
        // Arrange
        _tenantRepository.GetBySlugAsync("unknown-tenant", Arg.Any<CancellationToken>()).Returns((Tenant?)null);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new LoginCommand("unknown-tenant", "john@acme.com", "password"), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ThrowsUnauthorizedException()
    {
        // Arrange
        var tenant = Tenant.Create("Acme Corp", "acme-corp");
        _tenantRepository.GetBySlugAsync("acme-corp", Arg.Any<CancellationToken>()).Returns(tenant);
        _userRepository.GetByEmailAsync(tenant.Id, "ghost@acme.com", Arg.Any<CancellationToken>()).Returns((User?)null);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new LoginCommand("acme-corp", "ghost@acme.com", "password"), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_WhenPasswordIncorrect_ThrowsUnauthorizedException()
    {
        // Arrange
        var tenant = Tenant.Create("Acme Corp", "acme-corp");
        var user = ExistingUser(tenant.Id, "hashed-password");
        _tenantRepository.GetBySlugAsync("acme-corp", Arg.Any<CancellationToken>()).Returns(tenant);
        _userRepository.GetByEmailAsync(tenant.Id, "john@acme.com", Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify("wrong-password", "hashed-password").Returns(false);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new LoginCommand("acme-corp", "john@acme.com", "wrong-password"), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
