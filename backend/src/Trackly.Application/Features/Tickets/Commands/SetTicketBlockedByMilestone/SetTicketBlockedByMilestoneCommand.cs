using MediatR;

namespace Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByMilestone;

public sealed record SetTicketBlockedByMilestoneCommand(Guid TicketId, Guid? MilestoneId) : IRequest;
