using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByTicket;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.SetTicketBlockedByTicket;

public class SetTicketBlockedByTicketCommandHandlerTests
{
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private SetTicketBlockedByTicketCommandHandler CreateHandler() => new(_ticketRepository, _unitOfWork);

    private static Ticket ExistingTicket(Guid projectId) =>
        Ticket.Create(Guid.NewGuid(), projectId, "Set up Storybook", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithSameProjectBlockingTicket_SetsBlockedByTicketId()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticket = ExistingTicket(projectId);
        var blockingTicket = ExistingTicket(projectId);
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _ticketRepository.GetByIdAsync(blockingTicket.Id, Arg.Any<CancellationToken>()).Returns(blockingTicket);
        var handler = CreateHandler();
        var command = new SetTicketBlockedByTicketCommand(ticket.Id, blockingTicket.Id);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        ticket.BlockedByTicketId.Should().Be(blockingTicket.Id);
    }

    [Fact]
    public async Task Handle_WithNullBlockingTicketId_ClearsBlock()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticket = ExistingTicket(projectId);
        ticket.SetBlockedByTicket(Guid.NewGuid());
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        var handler = CreateHandler();
        var command = new SetTicketBlockedByTicketCommand(ticket.Id, null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        ticket.BlockedByTicketId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenBlockingTicketBelongsToADifferentProject_ThrowsConflictException()
    {
        // Arrange
        var ticket = ExistingTicket(Guid.NewGuid());
        var blockingTicket = ExistingTicket(Guid.NewGuid());
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _ticketRepository.GetByIdAsync(blockingTicket.Id, Arg.Any<CancellationToken>()).Returns(blockingTicket);
        var handler = CreateHandler();
        var command = new SetTicketBlockedByTicketCommand(ticket.Id, blockingTicket.Id);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }
}
