using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Meetings.Queries.GetWorkspaceMeetings;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Meetings.Queries.GetWorkspaceMeetings;

public class GetWorkspaceMeetingsQueryHandlerTests
{
    private readonly IMeetingRepository _meetingRepository = Substitute.For<IMeetingRepository>();

    private GetWorkspaceMeetingsQueryHandler CreateHandler() => new(_meetingRepository);

    [Fact]
    public async Task Handle_ReturnsMeetingsAcrossAllProjects()
    {
        // Arrange
        var projectAId = Guid.NewGuid();
        var projectBId = Guid.NewGuid();
        var meetingA = Meeting.Create(Guid.NewGuid(), projectAId, "Sprint Planning 14", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        var meetingB = Meeting.Create(Guid.NewGuid(), projectBId, "Client Sync", DateTime.UtcNow.AddDays(2), Guid.NewGuid());
        _meetingRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Meeting> { meetingA, meetingB });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceMeetingsQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(m => m.ProjectId == projectAId && m.Title == "Sprint Planning 14");
        result.Should().Contain(m => m.ProjectId == projectBId && m.Title == "Client Sync");
    }

    [Fact]
    public async Task Handle_WithNoMeetings_ReturnsEmptyList()
    {
        // Arrange
        _meetingRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Meeting>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceMeetingsQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
