namespace Trackly.Application.Features.Meetings.Queries.GetMeetingById;

public sealed record MeetingDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    DateTime ScheduledAt,
    string Notes,
    Guid CreatedById,
    DateTime CreatedAt,
    DateTime UpdatedAt);
