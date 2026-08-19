using Trackly.Domain.Common;

namespace Trackly.Domain.Entities;

public sealed class Milestone : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid ContractId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Deliberately no stored progress field — progress is always a live
    // query over Ticket.MilestoneId/Status, mirroring Project.IsArchived
    // being computed from ArchivedAt rather than a separately-tracked flag.

    private Milestone() { }

    public static Milestone Create(Guid tenantId, Guid projectId, Guid contractId, string title, Guid createdById)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (contractId == Guid.Empty)
            throw new ArgumentException("ContractId cannot be empty.", nameof(contractId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Milestone title cannot be empty.", nameof(title));

        if (createdById == Guid.Empty)
            throw new ArgumentException("CreatedById cannot be empty.", nameof(createdById));

        return new Milestone
        {
            TenantId = tenantId,
            ProjectId = projectId,
            ContractId = contractId,
            Title = title.Trim(),
            CreatedById = createdById,
            CreatedAt = DateTime.UtcNow
        };
    }
}
