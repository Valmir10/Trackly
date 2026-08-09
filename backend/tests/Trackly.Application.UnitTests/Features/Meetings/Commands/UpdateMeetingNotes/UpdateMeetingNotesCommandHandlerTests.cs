using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Meetings.Commands.UpdateMeetingNotes;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Meetings.Commands.UpdateMeetingNotes;

public class UpdateMeetingNotesCommandHandlerTests
{
    private readonly IMeetingRepository _meetingRepository = Substitute.For<IMeetingRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private UpdateMeetingNotesCommandHandler CreateHandler() => new(_meetingRepository, _unitOfWork);

    private static Meeting ExistingMeeting() =>
        Meeting.Create(Guid.NewGuid(), Guid.NewGuid(), "Sprint Planning 14", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

    [Fact]
    public async Task Handle_UpdatesNotesOnTheMeeting()
    {
        // Arrange
        var meeting = ExistingMeeting();
        _meetingRepository.GetByIdAsync(meeting.Id, Arg.Any<CancellationToken>()).Returns(meeting);
        var handler = CreateHandler();
        var command = new UpdateMeetingNotesCommand(meeting.Id, "Discussed the roadmap. >a1b2 Ship v2 by Friday.");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        meeting.Notes.Should().Be("Discussed the roadmap. >a1b2 Ship v2 by Friday.");
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenMeetingDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingMeetingId = Guid.NewGuid();
        _meetingRepository.GetByIdAsync(missingMeetingId, Arg.Any<CancellationToken>()).Returns((Meeting?)null);
        var handler = CreateHandler();
        var command = new UpdateMeetingNotesCommand(missingMeetingId, "Some notes");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
