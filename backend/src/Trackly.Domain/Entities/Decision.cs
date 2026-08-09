using Trackly.Domain.Common;
using Trackly.Domain.Events;

namespace Trackly.Domain.Entities;

public sealed class Decision : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid MeetingId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Decision() { }

    public static Decision Create(Guid tenantId, Guid projectId, Guid meetingId, string text, Guid createdById)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (meetingId == Guid.Empty)
            throw new ArgumentException("MeetingId cannot be empty.", nameof(meetingId));

        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Decision text cannot be empty.", nameof(text));

        if (createdById == Guid.Empty)
            throw new ArgumentException("CreatedById cannot be empty.", nameof(createdById));

        var now = DateTime.UtcNow;
        var decision = new Decision
        {
            TenantId = tenantId,
            ProjectId = projectId,
            MeetingId = meetingId,
            Text = text.Trim(),
            CreatedById = createdById,
            CreatedAt = now
        };

        decision.AddDomainEvent(new DecisionCreatedEvent(decision.Id, meetingId, projectId, decision.Text, createdById, now));
        return decision;
    }
}
