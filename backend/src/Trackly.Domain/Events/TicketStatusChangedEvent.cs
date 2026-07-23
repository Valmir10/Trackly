using Trackly.Domain.Common;
using Trackly.Domain.Enums;

namespace Trackly.Domain.Events;

public sealed record TicketStatusChangedEvent(Guid TicketId, Guid ProjectId, TicketStatus OldStatus, TicketStatus NewStatus) : IDomainEvent;
