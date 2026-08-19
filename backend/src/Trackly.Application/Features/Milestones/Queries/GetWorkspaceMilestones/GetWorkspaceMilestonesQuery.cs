using MediatR;
using Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;

namespace Trackly.Application.Features.Milestones.Queries.GetWorkspaceMilestones;

public sealed record GetWorkspaceMilestonesQuery : IRequest<IReadOnlyList<MilestoneDto>>;
