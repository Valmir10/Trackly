using MediatR;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Decisions.Queries.GetMeetingDecisions;

namespace Trackly.Application.Features.Decisions.Queries.GetWorkspaceDecisions;

public sealed class GetWorkspaceDecisionsQueryHandler : IRequestHandler<GetWorkspaceDecisionsQuery, IReadOnlyList<DecisionDto>>
{
    private readonly IDecisionRepository _decisionRepository;

    public GetWorkspaceDecisionsQueryHandler(IDecisionRepository decisionRepository)
    {
        _decisionRepository = decisionRepository;
    }

    public async Task<IReadOnlyList<DecisionDto>> Handle(GetWorkspaceDecisionsQuery request, CancellationToken cancellationToken)
    {
        var decisions = await _decisionRepository.GetAllAsync(cancellationToken);

        return decisions
            .Select(d => new DecisionDto(d.Id, d.MeetingId, d.ProjectId, d.Text, d.CreatedById, d.CreatedAt))
            .ToList();
    }
}
