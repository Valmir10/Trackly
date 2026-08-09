using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Decisions.Commands.CreateDecision;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Decisions.Commands.CreateDecision;

public class CreateDecisionCommandHandlerTests
{
    private readonly IDecisionRepository _decisionRepository = Substitute.For<IDecisionRepository>();
    private readonly IMeetingRepository _meetingRepository = Substitute.For<IMeetingRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private CreateDecisionCommandHandler CreateHandler() =>
        new(_decisionRepository, _meetingRepository, _unitOfWork, _currentTenant, _currentUser);

    private static Meeting ExistingMeeting(Guid projectId) =>
        Meeting.Create(Guid.NewGuid(), projectId, "Sprint Planning 14", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsNewDecisionId()
    {
        // Arrange
        var meeting = ExistingMeeting(Guid.NewGuid());
        _meetingRepository.GetByIdAsync(meeting.Id, Arg.Any<CancellationToken>()).Returns(meeting);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateDecisionCommand(meeting.Id, "Ship v2 by Friday");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_DerivesProjectAndTenantFromTheLoadedMeeting_NotFromTheRequest()
    {
        // Arrange — the command itself carries no ProjectId/TenantId at all,
        // which is the point: there is nothing on the request for a caller
        // to substitute. This test guards against a future refactor
        // accidentally introducing a client-trusted ProjectId/TenantId.
        var realTenantId = Guid.NewGuid();
        var realProjectId = Guid.NewGuid();
        var meeting = ExistingMeeting(realProjectId);
        _meetingRepository.GetByIdAsync(meeting.Id, Arg.Any<CancellationToken>()).Returns(meeting);
        _currentTenant.TenantId.Returns(realTenantId);
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateDecisionCommand(meeting.Id, "Ship v2 by Friday");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _decisionRepository.Received(1).AddAsync(
            Arg.Is<Decision>(d =>
                d.ProjectId == realProjectId &&
                d.TenantId == realTenantId &&
                d.MeetingId == meeting.Id &&
                d.Text == "Ship v2 by Friday"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenMeetingDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingMeetingId = Guid.NewGuid();
        _meetingRepository.GetByIdAsync(missingMeetingId, Arg.Any<CancellationToken>()).Returns((Meeting?)null);
        var handler = CreateHandler();
        var command = new CreateDecisionCommand(missingMeetingId, "Ship v2 by Friday");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
