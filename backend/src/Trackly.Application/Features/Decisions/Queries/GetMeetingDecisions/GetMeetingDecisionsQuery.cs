using MediatR;

namespace Trackly.Application.Features.Decisions.Queries.GetMeetingDecisions;

public sealed record GetMeetingDecisionsQuery(Guid MeetingId) : IRequest<IReadOnlyList<DecisionDto>>;
