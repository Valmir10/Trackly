using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Commands.AssignTicketToMilestone;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.AssignTicketToMilestone;

public class AssignTicketToMilestoneCommandHandlerTests
{
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IMilestoneRepository _milestoneRepository = Substitute.For<IMilestoneRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private AssignTicketToMilestoneCommandHandler CreateHandler() =>
        new(_ticketRepository, _milestoneRepository, _unitOfWork);

    private static Ticket ExistingTicket(Guid projectId) =>
        Ticket.Create(Guid.NewGuid(), projectId, "Set up Storybook", Guid.NewGuid());

    private static Milestone ExistingMilestone(Guid projectId) =>
        Milestone.Create(Guid.NewGuid(), projectId, Guid.NewGuid(), "Milestone 2", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithSameProjectMilestone_AssignsTicketToMilestone()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticket = ExistingTicket(projectId);
        var milestone = ExistingMilestone(projectId);
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _milestoneRepository.GetByIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(milestone);
        var handler = CreateHandler();
        var command = new AssignTicketToMilestoneCommand(ticket.Id, milestone.Id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        ticket.MilestoneId.Should().Be(milestone.Id);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNullMilestoneId_RemovesTicketFromMilestone()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticket = ExistingTicket(projectId);
        ticket.AssignToMilestone(Guid.NewGuid());
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        var handler = CreateHandler();
        var command = new AssignTicketToMilestoneCommand(ticket.Id, null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        ticket.MilestoneId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenMilestoneBelongsToADifferentProject_ThrowsConflictException()
    {
        // Arrange — the existing tenant-only query filters don't stop this
        // on their own; the handler's explicit check is the only guard.
        var ticket = ExistingTicket(Guid.NewGuid());
        var milestone = ExistingMilestone(Guid.NewGuid());
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _milestoneRepository.GetByIdAsync(milestone.Id, Arg.Any<CancellationToken>()).Returns(milestone);
        var handler = CreateHandler();
        var command = new AssignTicketToMilestoneCommand(ticket.Id, milestone.Id);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
        ticket.MilestoneId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingTicketId = Guid.NewGuid();
        _ticketRepository.GetByIdAsync(missingTicketId, Arg.Any<CancellationToken>()).Returns((Ticket?)null);
        var handler = CreateHandler();
        var command = new AssignTicketToMilestoneCommand(missingTicketId, Guid.NewGuid());

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WhenMilestoneDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var ticket = ExistingTicket(Guid.NewGuid());
        var missingMilestoneId = Guid.NewGuid();
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _milestoneRepository.GetByIdAsync(missingMilestoneId, Arg.Any<CancellationToken>()).Returns((Milestone?)null);
        var handler = CreateHandler();
        var command = new AssignTicketToMilestoneCommand(ticket.Id, missingMilestoneId);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
