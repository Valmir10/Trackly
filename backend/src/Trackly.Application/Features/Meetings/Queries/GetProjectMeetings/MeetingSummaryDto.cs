namespace Trackly.Application.Features.Meetings.Queries.GetProjectMeetings;

// Deliberately excludes Notes — a meetings list shouldn't ship every
// meeting's full notes payload on every load, the same reasoning
// TicketDto doesn't inline a project's entire chat history.
public sealed record MeetingSummaryDto(Guid Id, Guid ProjectId, string Title, DateTime ScheduledAt, DateTime CreatedAt);
