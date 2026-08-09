using MediatR;

namespace Trackly.Application.Features.Meetings.Queries.GetMeetingById;

public sealed record GetMeetingByIdQuery(Guid MeetingId) : IRequest<MeetingDto>;
