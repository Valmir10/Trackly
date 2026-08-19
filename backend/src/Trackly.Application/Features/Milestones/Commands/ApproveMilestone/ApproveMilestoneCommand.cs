using MediatR;

namespace Trackly.Application.Features.Milestones.Commands.ApproveMilestone;

// No ProjectId field — full stop. The approving project is always derived
// from ICurrentClientRoomService, never from client input.
public sealed record ApproveMilestoneCommand(Guid MilestoneId) : IRequest<Guid>;
