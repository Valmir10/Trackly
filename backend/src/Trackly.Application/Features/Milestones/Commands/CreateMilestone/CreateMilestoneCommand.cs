using MediatR;

namespace Trackly.Application.Features.Milestones.Commands.CreateMilestone;

public sealed record CreateMilestoneCommand(Guid ContractId, string Title) : IRequest<Guid>;
