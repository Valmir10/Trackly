using MediatR;
using Trackly.Domain.Enums;

namespace Trackly.Application.Features.Tickets.Commands.CreateTicket;

public sealed record CreateTicketCommand(
    Guid ProjectId,
    string Title,
    string? Description,
    TicketPriority Priority,
    Guid? AssignedToId,
    DateTime? DueDate) : IRequest<Guid>;
