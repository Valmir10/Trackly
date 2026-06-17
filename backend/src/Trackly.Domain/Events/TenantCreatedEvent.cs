using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record TenantCreatedEvent(Guid TenantId, string Name, string Slug) : IDomainEvent;
