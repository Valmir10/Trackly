using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Projects.Commands.CreateProject;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandlerTests
{
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private CreateProjectCommandHandler CreateHandler() =>
        new(_projectRepository, _unitOfWork, _currentTenant, _currentUser);

    // -------------------------------------------------------
    // Handle
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsNewProjectId()
    {
        // Arrange
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateProjectCommand(
            "Frontend redesign", "var(--tp-cat-1)", null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_PersistsProjectViaRepository()
    {
        // Arrange
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateProjectCommand(
            "Frontend redesign", "var(--tp-cat-1)", null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _projectRepository.Received(1).AddAsync(
            Arg.Is<Project>(p => p.Name == "Frontend redesign"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_SavesChanges()
    {
        // Arrange
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateProjectCommand(
            "Frontend redesign", "var(--tp-cat-1)", null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
