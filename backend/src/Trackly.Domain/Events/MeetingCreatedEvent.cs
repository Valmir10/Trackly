using Trackly.Domain.Common;

namespace Trackly.Domain.Events;

public sealed record MeetingCreatedEvent(Guid MeetingId, Guid ProjectId, string Title) : IDomainEvent;
