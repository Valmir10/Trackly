using Trackly.Domain.Common;
using Trackly.Domain.Events;

namespace Trackly.Domain.Entities;

public sealed class Project : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Color { get; private set; } = string.Empty;
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ArchivedAt { get; private set; }

    public bool IsArchived => ArchivedAt.HasValue;

    private Project() { }

    public static Project Create(Guid tenantId, string name, string color, Guid createdById, string? description = null)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name cannot be empty.", nameof(name));

        if (string.IsNullOrWhiteSpace(color))
            throw new ArgumentException("Color cannot be empty.", nameof(color));

        if (createdById == Guid.Empty)
            throw new ArgumentException("CreatedById cannot be empty.", nameof(createdById));

        var project = new Project
        {
            TenantId = tenantId,
            Name = name.Trim(),
            Color = color.Trim(),
            CreatedById = createdById,
            Description = description?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        project.AddDomainEvent(new ProjectCreatedEvent(project.Id, tenantId, project.Name));
        return project;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name cannot be empty.", nameof(name));

        Name = name.Trim();
    }

    public void UpdateDescription(string? description)
    {
        Description = description?.Trim();
    }

    public void Archive()
    {
        if (IsArchived) return;
        ArchivedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        ArchivedAt = null;
    }
}
