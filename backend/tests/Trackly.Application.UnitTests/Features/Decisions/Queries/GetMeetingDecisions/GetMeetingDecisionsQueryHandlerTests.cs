using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Decisions.Queries.GetMeetingDecisions;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Decisions.Queries.GetMeetingDecisions;

public class GetMeetingDecisionsQueryHandlerTests
{
    private readonly IDecisionRepository _decisionRepository = Substitute.For<IDecisionRepository>();

    private GetMeetingDecisionsQueryHandler CreateHandler() => new(_decisionRepository);

    [Fact]
    public async Task Handle_ReturnsDecisionsMappedToDtos()
    {
        // Arrange
        var meetingId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var decision = Decision.Create(Guid.NewGuid(), projectId, meetingId, "Ship v2 by Friday", Guid.NewGuid());
        _decisionRepository.GetByMeetingIdAsync(meetingId, Arg.Any<CancellationToken>()).Returns(new List<Decision> { decision });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetMeetingDecisionsQuery(meetingId), CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Id.Should().Be(decision.Id);
        result[0].MeetingId.Should().Be(meetingId);
        result[0].ProjectId.Should().Be(projectId);
        result[0].Text.Should().Be("Ship v2 by Friday");
    }

    [Fact]
    public async Task Handle_WithNoDecisions_ReturnsEmptyList()
    {
        // Arrange
        var meetingId = Guid.NewGuid();
        _decisionRepository.GetByMeetingIdAsync(meetingId, Arg.Any<CancellationToken>()).Returns(new List<Decision>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetMeetingDecisionsQuery(meetingId), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
