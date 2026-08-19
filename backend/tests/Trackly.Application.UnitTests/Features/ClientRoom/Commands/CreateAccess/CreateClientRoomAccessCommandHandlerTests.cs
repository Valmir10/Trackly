using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.ClientRoom.Commands.CreateAccess;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.ClientRoom.Commands.CreateAccess;

public class CreateClientRoomAccessCommandHandlerTests
{
    private readonly IClientRoomAccessRepository _accessRepository = Substitute.For<IClientRoomAccessRepository>();
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly IClientRoomTokenService _tokenService = Substitute.For<IClientRoomTokenService>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private CreateClientRoomAccessCommandHandler CreateHandler() =>
        new(_accessRepository, _projectRepository, _tokenService, _unitOfWork, _currentTenant, _currentUser);

    private static Project ExistingProject() =>
        Project.Create(Guid.NewGuid(), "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsAccessIdAndRawToken()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _tokenService.GenerateToken().Returns(("raw-token-value", "hashed-token-value"));
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new CreateClientRoomAccessCommand(project.Id), CancellationToken.None);

        // Assert
        result.AccessId.Should().NotBe(Guid.Empty);
        result.RawToken.Should().Be("raw-token-value");
    }

    [Fact]
    public async Task Handle_PersistsOnlyTheHash_NeverTheRawToken()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _tokenService.GenerateToken().Returns(("raw-token-value", "hashed-token-value"));
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();

        // Act
        await handler.Handle(new CreateClientRoomAccessCommand(project.Id), CancellationToken.None);

        // Assert
        await _accessRepository.Received(1).AddAsync(
            Arg.Is<ClientRoomAccess>(a => a.TokenHash == "hashed-token-value" && a.ProjectId == project.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingProjectId = Guid.NewGuid();
        _projectRepository.GetByIdAsync(missingProjectId, Arg.Any<CancellationToken>()).Returns((Project?)null);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new CreateClientRoomAccessCommand(missingProjectId), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
