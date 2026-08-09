using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Meetings.Commands.CreateMeeting;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Meetings.Commands.CreateMeeting;

public class CreateMeetingCommandHandlerTests
{
    private readonly IMeetingRepository _meetingRepository = Substitute.For<IMeetingRepository>();
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private CreateMeetingCommandHandler CreateHandler() =>
        new(_meetingRepository, _projectRepository, _unitOfWork, _currentTenant, _currentUser);

    private static Project ExistingProject() =>
        Project.Create(Guid.NewGuid(), "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsNewMeetingId()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateMeetingCommand(project.Id, "Sprint Planning 14", DateTime.UtcNow.AddDays(1));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_PersistsMeetingViaRepository()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateMeetingCommand(project.Id, "Sprint Planning 14", DateTime.UtcNow.AddDays(1));

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _meetingRepository.Received(1).AddAsync(
            Arg.Is<Meeting>(m => m.Title == "Sprint Planning 14" && m.ProjectId == project.Id),
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
        var command = new CreateMeetingCommand(missingProjectId, "Sprint Planning 14", DateTime.UtcNow.AddDays(1));

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
