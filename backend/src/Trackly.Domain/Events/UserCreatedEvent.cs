using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record UserCreatedEvent(Guid UserId, Guid TenantId, string Email) : IDomainEvent;
