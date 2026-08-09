using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Decisions.Commands.CreateDecision;

public sealed class CreateDecisionCommandHandler : IRequestHandler<CreateDecisionCommand, Guid>
{
    private readonly IDecisionRepository _decisionRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ICurrentUserService _currentUser;

    public CreateDecisionCommandHandler(
        IDecisionRepository decisionRepository,
        IMeetingRepository meetingRepository,
        IUnitOfWork unitOfWork,
        ICurrentTenantService currentTenant,
        ICurrentUserService currentUser)
    {
        _decisionRepository = decisionRepository;
        _meetingRepository = meetingRepository;
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateDecisionCommand request, CancellationToken cancellationToken)
    {
        var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, cancellationToken)
            ?? throw new NotFoundException(nameof(Meeting), request.MeetingId);

        // ProjectId/TenantId are always derived from the loaded meeting, never
        // trusted from the request — a decision can't be planted under a
        // project or tenant it doesn't actually belong to.
        var decision = Decision.Create(
            _currentTenant.TenantId,
            meeting.ProjectId,
            meeting.Id,
            request.Text,
            _currentUser.UserId);

        await _decisionRepository.AddAsync(decision, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return decision.Id;
    }
}
