using MediatR;

namespace Trackly.Application.Features.Tickets.Commands.AssignTicketToMilestone;

public sealed record AssignTicketToMilestoneCommand(Guid TicketId, Guid? MilestoneId) : IRequest;
