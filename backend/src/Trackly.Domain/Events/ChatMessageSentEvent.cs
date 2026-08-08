using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record ChatMessageSentEvent(
    Guid MessageId,
    Guid ProjectId,
    Guid? TicketId,
    Guid AuthorId,
    string Content,
    DateTime CreatedAt) : IDomainEvent;
