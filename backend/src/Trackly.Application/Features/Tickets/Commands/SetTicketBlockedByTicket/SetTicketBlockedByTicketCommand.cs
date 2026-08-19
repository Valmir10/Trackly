using MediatR;

namespace Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByTicket;

public sealed record SetTicketBlockedByTicketCommand(Guid TicketId, Guid? BlockingTicketId) : IRequest;
