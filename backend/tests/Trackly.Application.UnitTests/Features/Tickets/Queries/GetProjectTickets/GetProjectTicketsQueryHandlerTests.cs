using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Queries.GetProjectTickets;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Tickets.Queries.GetProjectTickets;

public class GetProjectTicketsQueryHandlerTests
{
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    private GetProjectTicketsQueryHandler CreateHandler() => new(_ticketRepository, _userRepository);

    private static Ticket ExistingTicket(Guid projectId, Guid? assignedToId = null)
    {
        var ticket = Ticket.Create(Guid.NewGuid(), projectId, "Set up Storybook", Guid.NewGuid());
        if (assignedToId is Guid id) ticket.AssignTo(id);
        return ticket;
    }

    [Fact]
    public async Task Handle_WithAssignedUser_ResolvesInitials()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var user = User.Create(Guid.NewGuid(), "sarah@acme.com", "Sarah", "Kim");
        var ticket = ExistingTicket(projectId, user.Id);
        _ticketRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Ticket> { ticket });
        _userRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectTicketsQuery(projectId), CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].AssignedToInitials.Should().Be("SK");
        result[0].ProjectId.Should().Be(projectId);
    }

    [Fact]
    public async Task Handle_WithNoAssignee_HasNullInitials()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var ticket = ExistingTicket(projectId);
        _ticketRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Ticket> { ticket });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectTicketsQuery(projectId), CancellationToken.None);

        // Assert
        result[0].AssignedToInitials.Should().BeNull();
        await _userRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithSameAssigneeOnMultipleTickets_ResolvesUserOnlyOnce()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var user = User.Create(Guid.NewGuid(), "sarah@acme.com", "Sarah", "Kim");
        var tickets = new List<Ticket> { ExistingTicket(projectId, user.Id), ExistingTicket(projectId, user.Id) };
        _ticketRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(tickets);
        _userRepository.GetByIdAsync(user.Id, Arg.Any<CancellationToken>()).Returns(user);
        var handler = CreateHandler();

        // Act
        await handler.Handle(new GetProjectTicketsQuery(projectId), CancellationToken.None);

        // Assert
        await _userRepository.Received(1).GetByIdAsync(user.Id, Arg.Any<CancellationToken>());
    }
}
