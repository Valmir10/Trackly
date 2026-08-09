using Trackly.Domain.Common;
using Trackly.Domain.Events;

namespace Trackly.Domain.Entities;

public sealed class Meeting : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public DateTime ScheduledAt { get; private set; }
    public string Notes { get; private set; } = string.Empty;
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Meeting() { }

    public static Meeting Create(Guid tenantId, Guid projectId, string title, DateTime scheduledAt, Guid createdById)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Meeting title cannot be empty.", nameof(title));

        if (createdById == Guid.Empty)
            throw new ArgumentException("CreatedById cannot be empty.", nameof(createdById));

        var now = DateTime.UtcNow;
        var meeting = new Meeting
        {
            TenantId = tenantId,
            ProjectId = projectId,
            Title = title.Trim(),
            ScheduledAt = scheduledAt,
            Notes = string.Empty,
            CreatedById = createdById,
            CreatedAt = now,
            UpdatedAt = now
        };

        meeting.AddDomainEvent(new MeetingCreatedEvent(meeting.Id, projectId, meeting.Title));
        return meeting;
    }

    public void UpdateNotes(string notes)
    {
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
}
