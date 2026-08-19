using MediatR;

namespace Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;

public sealed record GetProjectMilestonesQuery(Guid ProjectId) : IRequest<IReadOnlyList<MilestoneDto>>;
