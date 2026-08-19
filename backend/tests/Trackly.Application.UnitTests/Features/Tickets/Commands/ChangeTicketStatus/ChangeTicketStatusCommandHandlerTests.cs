using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Commands.ChangeTicketStatus;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.ChangeTicketStatus;

public class ChangeTicketStatusCommandHandlerTests
{
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private ChangeTicketStatusCommandHandler CreateHandler() => new(_ticketRepository, _unitOfWork);

    private static Ticket ExistingTicket() =>
        Ticket.Create(Guid.NewGuid(), Guid.NewGuid(), "Set up Storybook", Guid.NewGuid());

    // -------------------------------------------------------
    // Handle — happy path
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_MovesTicketToNewStatus()
    {
        // Arrange
        var ticket = ExistingTicket();
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        var handler = CreateHandler();
        var command = new ChangeTicketStatusCommand(ticket.Id, TicketStatus.Done, 0);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        ticket.Status.Should().Be(TicketStatus.Done);
        ticket.CompletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_SavesChanges()
    {
        // Arrange
        var ticket = ExistingTicket();
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        var handler = CreateHandler();
        var command = new ChangeTicketStatusCommand(ticket.Id, TicketStatus.InProgress, 1);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    // -------------------------------------------------------
    // Handle — missing ticket
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingTicketId = Guid.NewGuid();
        _ticketRepository.GetByIdAsync(missingTicketId, Arg.Any<CancellationToken>()).Returns((Ticket?)null);
        var handler = CreateHandler();
        var command = new ChangeTicketStatusCommand(missingTicketId, TicketStatus.Done, 0);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    // -------------------------------------------------------
    // Handle — cascade unblock (Move 8)
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WhenMovedToDone_ClearsBlockedByTicketIdOnDownstreamTickets()
    {
        // Arrange
        var ticket = ExistingTicket();
        var blockedTicket = ExistingTicket();
        blockedTicket.SetBlockedByTicket(ticket.Id);
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _ticketRepository.GetBlockedByTicketAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(new List<Ticket> { blockedTicket });
        var handler = CreateHandler();
        var command = new ChangeTicketStatusCommand(ticket.Id, TicketStatus.Done, 0);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        blockedTicket.BlockedByTicketId.Should().BeNull();
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenMovedOutOfDone_DoesNotQueryOrReblockDownstreamTickets()
    {
        // Arrange — one-directional: moving out of Done must not re-block
        // tickets that were already cleared by an earlier approval/completion.
        var ticket = ExistingTicket();
        ticket.ChangeStatus(TicketStatus.Done, 0);
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        var handler = CreateHandler();
        var command = new ChangeTicketStatusCommand(ticket.Id, TicketStatus.InProgress, 0);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _ticketRepository.DidNotReceive().GetBlockedByTicketAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }
}
