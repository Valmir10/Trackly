using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Commands.CreateTicket;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.CreateTicket;

public class CreateTicketCommandHandlerTests
{
    private readonly ITicketRepository _ticketRepository = Substitute.For<ITicketRepository>();
    private readonly IProjectRepository _projectRepository = Substitute.For<IProjectRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentTenantService _currentTenant = Substitute.For<ICurrentTenantService>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private CreateTicketCommandHandler CreateHandler() =>
        new(_ticketRepository, _projectRepository, _unitOfWork, _currentTenant, _currentUser);

    private static Project ExistingProject() =>
        Project.Create(Guid.NewGuid(), "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());

    // -------------------------------------------------------
    // Handle — happy path
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsNewTicketId()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateTicketCommand(project.Id, "Set up Storybook", null, TicketPriority.Medium, null, null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_PersistsTicketViaRepository()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateTicketCommand(project.Id, "Set up Storybook", null, TicketPriority.Medium, null, null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _ticketRepository.Received(1).AddAsync(
            Arg.Is<Ticket>(t => t.Title == "Set up Storybook" && t.ProjectId == project.Id),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithOriginMeetingId_PersistsItOnTheTicket()
    {
        // Arrange
        var project = ExistingProject();
        var meetingId = Guid.NewGuid();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateTicketCommand(project.Id, "Fix the flaky test", null, TicketPriority.Medium, null, null, meetingId);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _ticketRepository.Received(1).AddAsync(
            Arg.Is<Ticket>(t => t.OriginMeetingId == meetingId),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithoutOriginMeetingId_LeavesItNull()
    {
        // Arrange
        var project = ExistingProject();
        _projectRepository.GetByIdAsync(project.Id, Arg.Any<CancellationToken>()).Returns(project);
        _currentTenant.TenantId.Returns(Guid.NewGuid());
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateTicketCommand(project.Id, "Set up Storybook", null, TicketPriority.Medium, null, null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _ticketRepository.Received(1).AddAsync(
            Arg.Is<Ticket>(t => t.OriginMeetingId == null),
            Arg.Any<CancellationToken>());
    }

    // -------------------------------------------------------
    // Handle — missing project
    // -------------------------------------------------------

    [Fact]
    public async Task Handle_WhenProjectDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingProjectId = Guid.NewGuid();
        _projectRepository.GetByIdAsync(missingProjectId, Arg.Any<CancellationToken>()).Returns((Project?)null);
        var handler = CreateHandler();
        var command = new CreateTicketCommand(missingProjectId, "Set up Storybook", null, TicketPriority.Medium, null, null);

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
