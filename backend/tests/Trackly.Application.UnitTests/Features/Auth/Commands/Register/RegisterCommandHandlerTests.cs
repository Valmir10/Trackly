using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Auth.Commands.Register;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.UnitTests.Features.Auth.Commands.Register;

public class RegisterCommandHandlerTests
{
    private readonly ITenantRepository _tenantRepository = Substitute.For<ITenantRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly IJwtTokenService _jwtTokenService = Substitute.For<IJwtTokenService>();

    public RegisterCommandHandlerTests()
    {
        _tenantRepository.GetBySlugAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns((Tenant?)null);
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashed-password");
        _jwtTokenService.GenerateAccessToken(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>()).Returns("access-token");
        _jwtTokenService.GenerateRefreshToken().Returns(("raw-refresh-token", "hashed-refresh-token"));
    }

    private RegisterCommandHandler CreateHandler() => new(
        _tenantRepository, _userRepository, _refreshTokenRepository, _unitOfWork, _passwordHasher, _jwtTokenService);

    private static RegisterCommand ValidCommand() => new(
        "Acme Corp", "acme-corp", "john@acme.com", "supersecurepassword", "John", "Smith");

    // -------------------------------------------------------
    // Handle — happy path
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsAuthResult()
    {
        // Arrange
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(ValidCommand(), CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be("access-token");
        result.RefreshToken.Should().Be("raw-refresh-token");
        result.UserId.Should().NotBe(Guid.Empty);
        result.TenantId.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_CreatesUserWithOwnerRole()
    {
        // Arrange
        var handler = CreateHandler();

        // Act
        await handler.Handle(ValidCommand(), CancellationToken.None);

        // Assert
        await _userRepository.Received(1).AddAsync(
            Arg.Is<User>(u => u.Role == UserRole.Owner && u.Email == "john@acme.com"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_HashesThePasswordBeforeStoringIt()
    {
        // Arrange
        var handler = CreateHandler();

        // Act
        await handler.Handle(ValidCommand(), CancellationToken.None);

        // Assert
        _passwordHasher.Received(1).Hash("supersecurepassword");
        await _userRepository.Received(1).AddAsync(
            Arg.Is<User>(u => u.PasswordHash == "hashed-password"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_PersistsARefreshToken()
    {
        // Arrange
        var handler = CreateHandler();

        // Act
        await handler.Handle(ValidCommand(), CancellationToken.None);

        // Assert
        await _refreshTokenRepository.Received(1).AddAsync(Arg.Any<RefreshToken>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // -------------------------------------------------------
    // Handle — slug already taken
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WhenSlugAlreadyTaken_ThrowsConflictException()
    {
        // Arrange
        _tenantRepository.GetBySlugAsync("acme-corp", Arg.Any<CancellationToken>())
            .Returns(Tenant.Create("Acme Corp", "acme-corp"));
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(ValidCommand(), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }
}
