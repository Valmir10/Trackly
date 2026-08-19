using MediatR;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Enums;

namespace Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;

public sealed class GetProjectMilestonesQueryHandler : IRequestHandler<GetProjectMilestonesQuery, IReadOnlyList<MilestoneDto>>
{
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IApprovalRepository _approvalRepository;

    public GetProjectMilestonesQueryHandler(
        IMilestoneRepository milestoneRepository,
        ITicketRepository ticketRepository,
        IApprovalRepository approvalRepository)
    {
        _milestoneRepository = milestoneRepository;
        _ticketRepository = ticketRepository;
        _approvalRepository = approvalRepository;
    }

    public async Task<IReadOnlyList<MilestoneDto>> Handle(GetProjectMilestonesQuery request, CancellationToken cancellationToken)
    {
        var milestones = await _milestoneRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);
        var results = new List<MilestoneDto>(milestones.Count);

        foreach (var milestone in milestones)
        {
            var tickets = await _ticketRepository.GetByMilestoneIdAsync(milestone.Id, cancellationToken);
            var ticketsDone = tickets.Count(t => t.Status == TicketStatus.Done);
            var progressPercentage = tickets.Count > 0 ? (int)Math.Round(ticketsDone * 100.0 / tickets.Count) : 0;

            var approval = await _approvalRepository.GetByMilestoneIdAsync(milestone.Id, cancellationToken);

            results.Add(new MilestoneDto(
                milestone.Id,
                milestone.ContractId,
                milestone.Title,
                tickets.Count,
                ticketsDone,
                progressPercentage,
                approval is not null,
                approval?.ApprovedAt));
        }

        return results;
    }
}
