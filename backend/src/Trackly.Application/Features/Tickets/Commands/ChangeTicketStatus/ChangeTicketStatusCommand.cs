using MediatR;
using Trackly.Domain.Enums;

namespace Trackly.Application.Features.Tickets.Commands.ChangeTicketStatus;

public sealed record ChangeTicketStatusCommand(Guid TicketId, TicketStatus NewStatus, int Position) : IRequest;
