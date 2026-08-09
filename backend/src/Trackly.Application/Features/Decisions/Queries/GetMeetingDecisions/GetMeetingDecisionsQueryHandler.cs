using MediatR;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Application.Features.Decisions.Queries.GetMeetingDecisions;

public sealed class GetMeetingDecisionsQueryHandler : IRequestHandler<GetMeetingDecisionsQuery, IReadOnlyList<DecisionDto>>
{
    private readonly IDecisionRepository _decisionRepository;

    public GetMeetingDecisionsQueryHandler(IDecisionRepository decisionRepository)
    {
        _decisionRepository = decisionRepository;
    }

    public async Task<IReadOnlyList<DecisionDto>> Handle(GetMeetingDecisionsQuery request, CancellationToken cancellationToken)
    {
        var decisions = await _decisionRepository.GetByMeetingIdAsync(request.MeetingId, cancellationToken);

        return decisions
            .Select(d => new DecisionDto(d.Id, d.MeetingId, d.ProjectId, d.Text, d.CreatedById, d.CreatedAt))
            .ToList();
    }
}
