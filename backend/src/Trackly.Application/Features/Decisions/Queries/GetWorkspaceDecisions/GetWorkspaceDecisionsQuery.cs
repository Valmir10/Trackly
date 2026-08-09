using MediatR;
using Trackly.Application.Features.Decisions.Queries.GetMeetingDecisions;

namespace Trackly.Application.Features.Decisions.Queries.GetWorkspaceDecisions;

public sealed record GetWorkspaceDecisionsQuery : IRequest<IReadOnlyList<DecisionDto>>;
