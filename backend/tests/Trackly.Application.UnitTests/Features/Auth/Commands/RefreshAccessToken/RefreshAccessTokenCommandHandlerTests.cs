using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Auth.Commands.RefreshAccessToken;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Auth.Commands.RefreshAccessToken;

public class RefreshAccessTokenCommandHandlerTests
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IJwtTokenService _jwtTokenService = Substitute.For<IJwtTokenService>();

    private RefreshAccessTokenCommandHandler CreateHandler() => new(
        _refreshTokenRepository, _userRepository, _unitOfWork, _jwtTokenService);

    public RefreshAccessTokenCommandHandlerTests()
    {
        _jwtTokenService.HashRefreshToken("raw-token").Returns("hashed-token");
        _jwtTokenService.GenerateRefreshToken().Returns(("new-raw-token", "new-hashed-token"));
        _jwtTokenService.GenerateAccessToken(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>()).Returns("new-access-token");
    }

    // -------------------------------------------------------
    // Handle — happy path
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WithValidToken_ReturnsNewAuthResult()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "john@acme.com", "John", "Smith");
        var existingToken = RefreshToken.Create(user.Id, "hashed-token", DateTime.UtcNow.AddDays(1));
        _refreshTokenRepository.GetByHashAsync("hashed-token", Arg.Any<CancellationToken>()).Returns(existingToken);
        _userRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new RefreshAccessTokenCommand("raw-token"), CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be("new-access-token");
        result.RefreshToken.Should().Be("new-raw-token");
        result.UserId.Should().Be(user.Id);
    }

    [Fact]
    public async Task Handle_RotatesTheOldToken()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "john@acme.com", "John", "Smith");
        var existingToken = RefreshToken.Create(user.Id, "hashed-token", DateTime.UtcNow.AddDays(1));
        _refreshTokenRepository.GetByHashAsync("hashed-token", Arg.Any<CancellationToken>()).Returns(existingToken);
        _userRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);
        var handler = CreateHandler();

        // Act
        await handler.Handle(new RefreshAccessTokenCommand("raw-token"), CancellationToken.None);

        // Assert
        existingToken.IsRevoked.Should().BeTrue();
        existingToken.ReplacedByTokenId.Should().NotBeNull();
        await _refreshTokenRepository.Received(1).AddAsync(Arg.Any<RefreshToken>(), Arg.Any<CancellationToken>());
    }

    // -------------------------------------------------------
    // Handle — invalid token
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WhenTokenNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        _refreshTokenRepository.GetByHashAsync("hashed-token", Arg.Any<CancellationToken>()).Returns((RefreshToken?)null);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new RefreshAccessTokenCommand("raw-token"), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_WhenTokenAlreadyRevoked_ThrowsUnauthorizedException()
    {
        // Arrange
        var user = User.Create(Guid.NewGuid(), "john@acme.com", "John", "Smith");
        var existingToken = RefreshToken.Create(user.Id, "hashed-token", DateTime.UtcNow.AddDays(1));
        existingToken.Revoke();
        _refreshTokenRepository.GetByHashAsync("hashed-token", Arg.Any<CancellationToken>()).Returns(existingToken);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new RefreshAccessTokenCommand("raw-token"), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
