using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByMilestone;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.SetTicketBlockedByMilestone;

public class SetTicketBlockedByMilestoneCommandHandlerTests
{
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IMilestoneRepository _milestoneRepository = Substitute.For<IMilestoneRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private SetTicketBlockedByMilestoneCommandHandler CreateHandler() =>
        new(_ticketRepository, _milestoneRepository, _unitOfWork);

    private static Ticket ExistingTicket(Guid projectId) =>
        Ticket.Create(Guid.NewGuid(), projectId, "Set up Storybook", Guid.NewGuid());

    private static Milestone ExistingMilestone(Guid projectId) =>
        Milestone.Create(Guid.NewGuid(), projectId, Guid.NewGuid(), "Milestone 2", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithSameProjectMilestone_SetsBlockedByMilestoneId()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticket = ExistingTicket(projectId);
        var milestone = ExistingMilestone(projectId);
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _milestoneRepository.GetByIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(milestone);
        var handler = CreateHandler();
        var command = new SetTicketBlockedByMilestoneCommand(ticket.Id, milestone.Id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        ticket.BlockedByMilestoneId.Should().Be(milestone.Id);
    }

    [Fact]
    public async Task Handle_WithNullMilestoneId_ClearsBlock()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticket = ExistingTicket(projectId);
        ticket.SetBlockedByMilestone(Guid.NewGuid());
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        var handler = CreateHandler();
        var command = new SetTicketBlockedByMilestoneCommand(ticket.Id, null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        ticket.BlockedByMilestoneId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenMilestoneBelongsToADifferentProject_ThrowsConflictException()
    {
        // Arrange
        var ticket = ExistingTicket(Guid.NewGuid());
        var milestone = ExistingMilestone(Guid.NewGuid());
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _milestoneRepository.GetByIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(milestone);
        var handler = CreateHandler();
        var command = new SetTicketBlockedByMilestoneCommand(ticket.Id, milestone.Id);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }
}
