using Trackly.Domain.Enums;

namespace Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

public sealed record TicketDto(
    Guid Id,
    string Title,
    string? Description,
    TicketStatus Status,
    TicketPriority Priority,
    Guid? AssignedToId,
    string? AssignedToInitials,
    DateTime? DueDate,
    int Position);
