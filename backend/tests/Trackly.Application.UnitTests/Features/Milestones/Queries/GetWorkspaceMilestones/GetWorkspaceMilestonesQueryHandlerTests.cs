using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Milestones.Queries.GetWorkspaceMilestones;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.UnitTests.Features.Milestones.Queries.GetWorkspaceMilestones;

public class GetWorkspaceMilestonesQueryHandlerTests
{
    private readonly IMilestoneRepository _milestoneRepository = Substitute.For<IMilestoneRepository>();
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IApprovalRepository _approvalRepository = Substitute.For<IApprovalRepository>();

    private GetWorkspaceMilestonesQueryHandler CreateHandler() =>
        new(_milestoneRepository, _ticketRepository, _approvalRepository);

    [Fact]
    public async Task Handle_ReturnsMilestonesAcrossAllProjects_WithComputedProgress()
    {
        // Arrange
        var projectAId = Guid.NewGuid();
        var projectBId = Guid.NewGuid();
        var milestoneA = Milestone.Create(Guid.NewGuid(), projectAId, Guid.NewGuid(), "Milestone A", Guid.NewGuid());
        var milestoneB = Milestone.Create(Guid.NewGuid(), projectBId, Guid.NewGuid(), "Milestone B", Guid.NewGuid());
        _milestoneRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Milestone> { milestoneA, milestoneB });

        var doneTicket = Ticket.Create(Guid.NewGuid(), projectAId, "Ship it", Guid.NewGuid());
        doneTicket.ChangeStatus(TicketStatus.Done, 0);
        _ticketRepository.GetByMilestoneIdAsync(milestoneA.Id, Arg.Any<CancellationToken>()).Returns(new List<Ticket> { doneTicket });
        _ticketRepository.GetByMilestoneIdAsync(milestoneB.Id, Arg.Any<CancellationToken>()).Returns(new List<Ticket>());

        _approvalRepository.GetByMilestoneIdAsync(milestoneA.Id, Arg.Any<CancellationToken>()).Returns((Approval?)null);
        _approvalRepository.GetByMilestoneIdAsync(milestoneB.Id, Arg.Any<CancellationToken>()).Returns((Approval?)null);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceMilestonesQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(m => m.Id == milestoneA.Id && m.ProjectId == projectAId && m.ProgressPercentage == 100);
        result.Should().Contain(m => m.Id == milestoneB.Id && m.ProjectId == projectBId && m.TicketsTotal == 0);
    }

    [Fact]
    public async Task Handle_WithNoMilestones_ReturnsEmptyList()
    {
        // Arrange
        _milestoneRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Milestone>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceMilestonesQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
