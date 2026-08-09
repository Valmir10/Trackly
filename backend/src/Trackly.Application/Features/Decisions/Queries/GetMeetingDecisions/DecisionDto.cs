namespace Trackly.Application.Features.Decisions.Queries.GetMeetingDecisions;

public sealed record DecisionDto(Guid Id, Guid MeetingId, Guid ProjectId, string Text, Guid CreatedById, DateTime CreatedAt);
