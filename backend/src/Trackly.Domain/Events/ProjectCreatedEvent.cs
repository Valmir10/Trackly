using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record ProjectCreatedEvent(Guid ProjectId, Guid TenantId, string Name) : IDomainEvent;
