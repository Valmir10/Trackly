using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record TicketCreatedEvent(Guid TicketId, Guid ProjectId, string Title) : IDomainEvent;
