using Trackly.Domain.Common;

namespace Trackly.Domain.Entities;

public sealed class Contract : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    // Object-storage key for the PDF, evidence rather than source of truth.
    // Unused until MinIO integration lands — real value never in Phase A.
    public string? PdfObjectKey { get; private set; }
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Contract() { }

    public static Contract Create(Guid tenantId, Guid projectId, string title, Guid createdById)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Contract title cannot be empty.", nameof(title));

        if (createdById == Guid.Empty)
            throw new ArgumentException("CreatedById cannot be empty.", nameof(createdById));

        return new Contract
        {
            TenantId = tenantId,
            ProjectId = projectId,
            Title = title.Trim(),
            CreatedById = createdById,
            CreatedAt = DateTime.UtcNow
        };
    }
}
