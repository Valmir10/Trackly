using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Chat.Commands.SendChatMessage;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Chat.Commands.SendChatMessage;

public class SendChatMessageCommandHandlerTests
{
    private readonly IChatMessageRepository _chatMessageRepository = Substitute.For<IChatMessageRepository>();
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private SendChatMessageCommandHandler CreateHandler() =>
        new(_chatMessageRepository, _projectRepository, _ticketRepository, _unitOfWork, _currentTenant, _currentUser);

    private static Project ExistingProject() =>
        Project.Create(Guid.NewGuid(), "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());

    private static Ticket ExistingTicket(Guid projectId) =>
        Ticket.Create(Guid.NewGuid(), projectId, "Set up Storybook", Guid.NewGuid());

    // -------------------------------------------------------
    // Handle — project-scoped (no ticket)
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WithoutTicketId_PersistsProjectScopedMessage()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new SendChatMessageCommand(project.Id, null, "Started on #127 today.");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        await _chatMessageRepository.Received(1).AddAsync(
            Arg.Is<ChatMessage>(m => m.TicketId == null && m.ProjectId == project.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingProjectId = Guid.NewGuid();
        _projectRepository.GetByIdAsync(missingProjectId, Arg.Any<CancellationToken>()).Returns((Project?)null);
        var handler = CreateHandler();
        var command = new SendChatMessageCommand(missingProjectId, null, "hello");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    // -------------------------------------------------------
    // Handle — ticket-scoped
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WithTicketId_PersistsTicketScopedMessage()
    {
        // Arrange
        var project = ExistingProject();
        var ticket = ExistingTicket(project.Id);
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _ticketRepository.GetByIdAsync(ticket.Id, Arg.Any<CancellationToken>()).Returns(ticket);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new SendChatMessageCommand(project.Id, ticket.Id, "Storybook config is in.");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _chatMessageRepository.Received(1).AddAsync(
            Arg.Is<ChatMessage>(m => m.TicketId == ticket.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithTicketId_WhenTicketDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var project = ExistingProject();
        var missingTicketId = Guid.NewGuid();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _ticketRepository.GetByIdAsync(missingTicketId, Arg.Any<CancellationToken>()).Returns((Ticket?)null);
        var handler = CreateHandler();
        var command = new SendChatMessageCommand(project.Id, missingTicketId, "hello");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
