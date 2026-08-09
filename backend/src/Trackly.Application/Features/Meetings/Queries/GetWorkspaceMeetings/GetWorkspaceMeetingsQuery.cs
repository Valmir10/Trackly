using MediatR;
using Trackly.Application.Features.Meetings.Queries.GetProjectMeetings;

namespace Trackly.Application.Features.Meetings.Queries.GetWorkspaceMeetings;

public sealed record GetWorkspaceMeetingsQuery : IRequest<IReadOnlyList<MeetingSummaryDto>>;
