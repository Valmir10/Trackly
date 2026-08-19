using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Contracts.Commands.CreateContract;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandHandlerTests
{
    private readonly IContractRepository _contractRepository = Substitute.For<IContractRepository>();
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private CreateContractCommandHandler CreateHandler() =>
        new(_contractRepository, _projectRepository, _unitOfWork, _currentTenant, _currentUser);

    private static Project ExistingProject() =>
        Project.Create(Guid.NewGuid(), "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsNewContractId()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateContractCommand(project.Id, "Meridian SOW");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_PersistsContractViaRepository()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateContractCommand(project.Id, "Meridian SOW");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _contractRepository.Received(1).AddAsync(
            Arg.Is<Contract>(c => c.Title == "Meridian SOW" && c.ProjectId == project.Id),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingProjectId = Guid.NewGuid();
        _projectRepository.GetByIdAsync(missingProjectId, Arg.Any<CancellationToken>()).Returns((Project?)null);
        var handler = CreateHandler();
        var command = new CreateContractCommand(missingProjectId, "Meridian SOW");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
