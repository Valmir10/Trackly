using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.UnitTests.Features.Milestones.Queries.GetProjectMilestones;

public class GetProjectMilestonesQueryHandlerTests
{
    private readonly IMilestoneRepository _milestoneRepository = Substitute.For<IMilestoneRepository>();
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IApprovalRepository _approvalRepository = Substitute.For<IApprovalRepository>();

    private GetProjectMilestonesQueryHandler CreateHandler() =>
        new(_milestoneRepository, _ticketRepository, _approvalRepository);

    [Fact]
    public async Task Handle_ReturnsMilestonesForTheProject_WithComputedProgressAndApproval()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var milestone = Milestone.Create(Guid.NewGuid(), projectId, Guid.NewGuid(), "Milestone 1", Guid.NewGuid());
        _milestoneRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Milestone> { milestone });

        var doneTicket = Ticket.Create(Guid.NewGuid(), projectId, "Ship it", Guid.NewGuid());
        doneTicket.ChangeStatus(TicketStatus.Done, 0);
        var openTicket = Ticket.Create(Guid.NewGuid(), projectId, "Polish it", Guid.NewGuid());
        _ticketRepository.GetByMilestoneIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(new List<Ticket> { doneTicket, openTicket });

        var approval = Approval.Create(Guid.NewGuid(), projectId, milestone.Id, Guid.NewGuid());
        _approvalRepository.GetByMilestoneIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(approval);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectMilestonesQuery(projectId), CancellationToken.None);

        // Assert
        var dto = result.Should().ContainSingle().Subject;
        dto.ProjectId.Should().Be(projectId);
        dto.ContractId.Should().Be(milestone.ContractId);
        dto.TicketsTotal.Should().Be(2);
        dto.TicketsDone.Should().Be(1);
        dto.ProgressPercentage.Should().Be(50);
        dto.IsApproved.Should().BeTrue();
        dto.ApprovedAt.Should().Be(approval.ApprovedAt);
    }

    [Fact]
    public async Task Handle_WithNoMilestones_ReturnsEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _milestoneRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Milestone>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectMilestonesQuery(projectId), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
