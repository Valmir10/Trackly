using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record DecisionCreatedEvent(
    Guid DecisionId,
    Guid MeetingId,
    Guid ProjectId,
    string Text,
    Guid CreatedById,
    DateTime CreatedAt) : IDomainEvent;
