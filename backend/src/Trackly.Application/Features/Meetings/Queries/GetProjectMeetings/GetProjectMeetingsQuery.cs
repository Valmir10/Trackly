using MediatR;

namespace Trackly.Application.Features.Meetings.Queries.GetProjectMeetings;

public sealed record GetProjectMeetingsQuery(Guid ProjectId) : IRequest<IReadOnlyList<MeetingSummaryDto>>;
