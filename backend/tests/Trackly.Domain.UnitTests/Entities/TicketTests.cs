using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class TicketTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidCreatedById = Guid.NewGuid();

    // -------------------------------------------------------
    // Ticket.Create
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsTicketWithCorrectProperties()
    {
        // Act
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);

        // Assert
        ticket.Id.Should().NotBe(Guid.Empty);
        ticket.TenantId.Should().Be(ValidTenantId);
        ticket.ProjectId.Should().Be(ValidProjectId);
        ticket.Title.Should().Be("Set up Storybook");
        ticket.CreatedById.Should().Be(ValidCreatedById);
        ticket.Status.Should().Be(TicketStatus.ToDo);
        ticket.Priority.Should().Be(TicketPriority.Medium);
        ticket.AssignedToId.Should().BeNull();
        ticket.CompletedAt.Should().BeNull();
        ticket.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        ticket.UpdatedAt.Should().Be(ticket.CreatedAt);
        ticket.OriginMeetingId.Should().BeNull();
    }

    [Fact]
    public void Create_WithOriginMeetingId_PersistsItOnTheTicket()
    {
        // Arrange
        var meetingId = Guid.NewGuid();

        // Act
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Fix the flaky test", ValidCreatedById, originMeetingId: meetingId);

        // Assert
        ticket.OriginMeetingId.Should().Be(meetingId);
    }

    [Fact]
    public void Create_AlwaysStartsInToDoRegardlessOfArguments()
    {
        // Act
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById, priority: TicketPriority.High);

        // Assert
        ticket.Status.Should().Be(TicketStatus.ToDo);
    }

    [Fact]
    public void Create_RaisesTicketCreatedEvent()
    {
        // Act
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);

        // Assert
        ticket.DomainEvents.Should().ContainSingle(e => e is TicketCreatedEvent);

        var domainEvent = ticket.DomainEvents.Single() as TicketCreatedEvent;
        domainEvent!.TicketId.Should().Be(ticket.Id);
        domainEvent.ProjectId.Should().Be(ValidProjectId);
        domainEvent.Title.Should().Be("Set up Storybook");
    }

    [Fact]
    public void Create_WithEmptyTitle_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Ticket.Create(ValidTenantId, ValidProjectId, "", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // ChangeStatus
    // -------------------------------------------------------

    [Fact]
    public void ChangeStatus_MovesToNewStatusAndPosition()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);

        // Act
        ticket.ChangeStatus(TicketStatus.InProgress, position: 2);

        // Assert
        ticket.Status.Should().Be(TicketStatus.InProgress);
        ticket.Position.Should().Be(2);
    }

    [Fact]
    public void ChangeStatus_ToDone_SetsCompletedAt()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);

        // Act
        ticket.ChangeStatus(TicketStatus.Done, position: 0);

        // Assert
        ticket.CompletedAt.Should().NotBeNull();
        ticket.CompletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void ChangeStatus_AwayFromDone_ClearsCompletedAt()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);
        ticket.ChangeStatus(TicketStatus.Done, position: 0);

        // Act
        ticket.ChangeStatus(TicketStatus.InReview, position: 0);

        // Assert
        ticket.CompletedAt.Should().BeNull();
    }

    [Fact]
    public void ChangeStatus_RaisesTicketStatusChangedEvent()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);

        // Act
        ticket.ChangeStatus(TicketStatus.InProgress, position: 0);

        // Assert
        var domainEvent = ticket.DomainEvents.OfType<TicketStatusChangedEvent>().Single();
        domainEvent.TicketId.Should().Be(ticket.Id);
        domainEvent.ProjectId.Should().Be(ValidProjectId);
        domainEvent.OldStatus.Should().Be(TicketStatus.ToDo);
        domainEvent.NewStatus.Should().Be(TicketStatus.InProgress);
    }

    [Fact]
    public void ChangeStatus_WithSameStatusAndPosition_DoesNotRaiseEvent()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);
        ticket.ClearDomainEvents();

        // Act
        ticket.ChangeStatus(TicketStatus.ToDo, position: 0);

        // Assert
        ticket.DomainEvents.Should().BeEmpty();
    }

    // -------------------------------------------------------
    // Rename / UpdateDescription / SetPriority
    // -------------------------------------------------------

    [Fact]
    public void Rename_WithValidTitle_UpdatesTitle()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);

        // Act
        ticket.Rename("Set up Storybook with addon-docs");

        // Assert
        ticket.Title.Should().Be("Set up Storybook with addon-docs");
    }

    [Fact]
    public void SetPriority_UpdatesPriority()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);

        // Act
        ticket.SetPriority(TicketPriority.High);

        // Assert
        ticket.Priority.Should().Be(TicketPriority.High);
    }

    // -------------------------------------------------------
    // AssignTo / Unassign
    // -------------------------------------------------------

    [Fact]
    public void AssignTo_SetsAssignedToId()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);
        var userId = Guid.NewGuid();

        // Act
        ticket.AssignTo(userId);

        // Assert
        ticket.AssignedToId.Should().Be(userId);
    }

    [Fact]
    public void Unassign_ClearsAssignedToId()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);
        ticket.AssignTo(Guid.NewGuid());

        // Act
        ticket.Unassign();

        // Assert
        ticket.AssignedToId.Should().BeNull();
    }

    // -------------------------------------------------------
    // SetDueDate / ClearDueDate
    // -------------------------------------------------------

    [Fact]
    public void SetDueDate_SetsDueDate()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);
        var dueDate = DateTime.UtcNow.AddDays(7);

        // Act
        ticket.SetDueDate(dueDate);

        // Assert
        ticket.DueDate.Should().Be(dueDate);
    }

    [Fact]
    public void ClearDueDate_ClearsDueDate()
    {
        // Arrange
        var ticket = Ticket.Create(ValidTenantId, ValidProjectId, "Set up Storybook", ValidCreatedById);
        ticket.SetDueDate(DateTime.UtcNow.AddDays(7));

        // Act
        ticket.ClearDueDate();

        // Assert
        ticket.DueDate.Should().BeNull();
    }
}
