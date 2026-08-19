using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record MilestoneApprovedEvent(
    Guid ApprovalId,
    Guid MilestoneId,
    Guid ProjectId,
    Guid TenantId,
    DateTime ApprovedAt) : IDomainEvent;
