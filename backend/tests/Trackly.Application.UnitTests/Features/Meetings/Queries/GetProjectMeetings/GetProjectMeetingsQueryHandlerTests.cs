using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Meetings.Queries.GetProjectMeetings;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Meetings.Queries.GetProjectMeetings;

public class GetProjectMeetingsQueryHandlerTests
{
    private readonly IMeetingRepository _meetingRepository = Substitute.For<IMeetingRepository>();

    private GetProjectMeetingsQueryHandler CreateHandler() => new(_meetingRepository);

    [Fact]
    public async Task Handle_ReturnsMeetingsMappedToSummaryDtos()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var meeting = Meeting.Create(Guid.NewGuid(), projectId, "Sprint Planning 14", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        _meetingRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Meeting> { meeting });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectMeetingsQuery(projectId), CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Id.Should().Be(meeting.Id);
        result[0].ProjectId.Should().Be(projectId);
        result[0].Title.Should().Be("Sprint Planning 14");
    }

    [Fact]
    public async Task Handle_WithNoMeetings_ReturnsEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _meetingRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Meeting>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectMeetingsQuery(projectId), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
