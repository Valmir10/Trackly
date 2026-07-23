using Trackly.Domain.Common;
using Trackly.Domain.Enums;
using Trackly.Domain.Events;

namespace Trackly.Domain.Entities;

public sealed class Ticket : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public TicketStatus Status { get; private set; }
    public TicketPriority Priority { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public Guid CreatedById { get; private set; }
    public DateTime? DueDate { get; private set; }
    public int Position { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Ticket() { }

    public static Ticket Create(
        Guid tenantId,
        Guid projectId,
        string title,
        Guid createdById,
        TicketPriority priority = TicketPriority.Medium,
        Guid? assignedToId = null,
        DateTime? dueDate = null,
        string? description = null,
        int position = 0)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Ticket title cannot be empty.", nameof(title));

        if (createdById == Guid.Empty)
            throw new ArgumentException("CreatedById cannot be empty.", nameof(createdById));

        var now = DateTime.UtcNow;
        var ticket = new Ticket
        {
            TenantId = tenantId,
            ProjectId = projectId,
            Title = title.Trim(),
            Description = description?.Trim(),
            Status = TicketStatus.ToDo,
            Priority = priority,
            AssignedToId = assignedToId,
            CreatedById = createdById,
            DueDate = dueDate,
            Position = position,
            CreatedAt = now,
            UpdatedAt = now
        };

        ticket.AddDomainEvent(new TicketCreatedEvent(ticket.Id, projectId, ticket.Title));
        return ticket;
    }

    public void ChangeStatus(TicketStatus newStatus, int position)
    {
        if (newStatus == Status && position == Position) return;

        var oldStatus = Status;
        Status = newStatus;
        Position = position;
        CompletedAt = newStatus == TicketStatus.Done ? DateTime.UtcNow : null;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TicketStatusChangedEvent(Id, ProjectId, oldStatus, newStatus));
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Ticket title cannot be empty.", nameof(title));

        Title = title.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPriority(TicketPriority priority)
    {
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignTo(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));

        AssignedToId = userId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unassign()
    {
        AssignedToId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearDueDate()
    {
        DueDate = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
