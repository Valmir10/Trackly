namespace Trackly.Application.Features.ClientRoom.Queries.GetSummary;

// Hand-built from scratch, never derived from the internal ProjectDto/
// TicketDto/MilestoneDto — deny-by-default. Aggregate-only: no individual
// ticket titles, assignees, or statuses reach this DTO at all.
public sealed record ClientRoomSummaryDto(string ProjectName, string ProjectColor, IReadOnlyList<ClientRoomContractDto> Contracts);

public sealed record ClientRoomContractDto(Guid Id, string Title, IReadOnlyList<ClientRoomMilestoneDto> Milestones);

public sealed record ClientRoomMilestoneDto(
    Guid Id,
    string Title,
    int TicketsTotal,
    int TicketsDone,
    int ProgressPercentage,
    bool IsApproved,
    DateTime? ApprovedAt);
