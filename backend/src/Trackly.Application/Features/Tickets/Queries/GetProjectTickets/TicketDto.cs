using Trackly.Domain.Enums;

namespace Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

public sealed record TicketDto(
    Guid Id,
    Guid ProjectId,
    string Title,
    string? Description,
    TicketStatus Status,
    TicketPriority Priority,
    Guid? AssignedToId,
    string? AssignedToInitials,
    DateTime? DueDate,
    int Position,
    DateTime CreatedAt,
    DateTime? CompletedAt,
    DateTime UpdatedAt,
    Guid? MilestoneId,
    Guid? BlockedByTicketId,
    Guid? BlockedByMilestoneId,
    Guid? OriginMeetingId);
