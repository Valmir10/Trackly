using Trackly.Domain.Common;
using Trackly.Domain.Events;

namespace Trackly.Domain.Entities;

public sealed class Approval : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid MilestoneId { get; private set; }
    // Approved by a client-room grant, not a User — there is deliberately
    // no user reference here. A client approving a milestone never has an
    // internal account.
    public Guid ClientRoomAccessId { get; private set; }
    public DateTime ApprovedAt { get; private set; }

    private Approval() { }

    public static Approval Create(Guid tenantId, Guid projectId, Guid milestoneId, Guid clientRoomAccessId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (milestoneId == Guid.Empty)
            throw new ArgumentException("MilestoneId cannot be empty.", nameof(milestoneId));

        if (clientRoomAccessId == Guid.Empty)
            throw new ArgumentException("ClientRoomAccessId cannot be empty.", nameof(clientRoomAccessId));

        var now = DateTime.UtcNow;
        var approval = new Approval
        {
            TenantId = tenantId,
            ProjectId = projectId,
            MilestoneId = milestoneId,
            ClientRoomAccessId = clientRoomAccessId,
            ApprovedAt = now
        };

        approval.AddDomainEvent(new MilestoneApprovedEvent(approval.Id, milestoneId, projectId, tenantId, now));
        return approval;
    }
}
