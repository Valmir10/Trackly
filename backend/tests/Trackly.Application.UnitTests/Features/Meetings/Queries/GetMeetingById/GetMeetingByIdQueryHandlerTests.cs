using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Meetings.Queries.GetMeetingById;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Meetings.Queries.GetMeetingById;

public class GetMeetingByIdQueryHandlerTests
{
    private readonly IMeetingRepository _meetingRepository = Substitute.For<IMeetingRepository>();

    private GetMeetingByIdQueryHandler CreateHandler() => new(_meetingRepository);

    [Fact]
    public async Task Handle_WithExistingMeeting_ReturnsMappedDto()
    {
        // Arrange
        var meeting = Meeting.Create(Guid.NewGuid(), Guid.NewGuid(), "Sprint Planning 14", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        meeting.UpdateNotes("Discussed the roadmap.");
        _meetingRepository.GetByIdAsync(meeting.Id, Arg.Any<CancellationToken>()).Returns(meeting);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetMeetingByIdQuery(meeting.Id), CancellationToken.None);

        // Assert
        result.Id.Should().Be(meeting.Id);
        result.ProjectId.Should().Be(meeting.ProjectId);
        result.Title.Should().Be("Sprint Planning 14");
        result.Notes.Should().Be("Discussed the roadmap.");
    }

    [Fact]
    public async Task Handle_WhenMeetingDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingMeetingId = Guid.NewGuid();
        _meetingRepository.GetByIdAsync(missingMeetingId, Arg.Any<CancellationToken>()).Returns((Meeting?)null);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new GetMeetingByIdQuery(missingMeetingId), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
