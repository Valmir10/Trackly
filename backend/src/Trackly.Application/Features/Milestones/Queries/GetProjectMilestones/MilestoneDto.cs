namespace Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;

public sealed record MilestoneDto(
    Guid Id,
    Guid ContractId,
    string Title,
    int TicketsTotal,
    int TicketsDone,
    int ProgressPercentage,
    bool IsApproved,
    DateTime? ApprovedAt);
