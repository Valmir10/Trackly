using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Queries.GetWorkspaceTickets;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Tickets.Queries.GetWorkspaceTickets;

public class GetWorkspaceTicketsQueryHandlerTests
{
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();

    private GetWorkspaceTicketsQueryHandler CreateHandler() => new(_ticketRepository, _userRepository);

    [Fact]
    public async Task Handle_ReturnsTicketsAcrossAllProjectsWithProjectId()
    {
        // Arrange
        var projectAId = Guid.NewGuid();
        var projectBId = Guid.NewGuid();
        var ticketA = Ticket.Create(Guid.NewGuid(), projectAId, "Set up Storybook", Guid.NewGuid());
        var ticketB = Ticket.Create(Guid.NewGuid(), projectBId, "Fix auth bug", Guid.NewGuid());
        _ticketRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Ticket> { ticketA, ticketB });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceTicketsQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.ProjectId == projectAId && t.Title == "Set up Storybook");
        result.Should().Contain(t => t.ProjectId == projectBId && t.Title == "Fix auth bug");
    }

    [Fact]
    public async Task Handle_WithNoTickets_ReturnsEmptyList()
    {
        // Arrange
        _ticketRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Ticket>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetWorkspaceTicketsQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
