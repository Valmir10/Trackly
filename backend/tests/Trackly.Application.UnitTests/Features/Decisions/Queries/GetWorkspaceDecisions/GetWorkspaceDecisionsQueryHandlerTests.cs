using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Decisions.Queries.GetWorkspaceDecisions;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Decisions.Queries.GetWorkspaceDecisions;

public class GetWorkspaceDecisionsQueryHandlerTests
{
    private readonly IDecisionRepository _decisionRepository = Substitute.For<IDecisionRepository>();

    private GetWorkspaceDecisionsQueryHandler CreateHandler() => new(_decisionRepository);

    [Fact]
    public async Task Handle_ReturnsDecisionsAcrossAllMeetings()
    {
        // Arrange
        var meetingAId = Guid.NewGuid();
        var meetingBId = Guid.NewGuid();
        var decisionA = Decision.Create(Guid.NewGuid(), Guid.NewGuid(), meetingAId, "Ship v2 by Friday", Guid.NewGuid());
        var decisionB = Decision.Create(Guid.NewGuid(), Guid.NewGuid(), meetingBId, "Adopt the new design system", Guid.NewGuid());
        _decisionRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Decision> { decisionA, decisionB });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceDecisionsQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(d => d.MeetingId == meetingAId && d.Text == "Ship v2 by Friday");
        result.Should().Contain(d => d.MeetingId == meetingBId && d.Text == "Adopt the new design system");
    }

    [Fact]
    public async Task Handle_WithNoDecisions_ReturnsEmptyList()
    {
        // Arrange
        _decisionRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Decision>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceDecisionsQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
