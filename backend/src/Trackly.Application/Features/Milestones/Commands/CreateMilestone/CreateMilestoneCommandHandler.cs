using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Milestones.Commands.CreateMilestone;

public sealed class CreateMilestoneCommandHandler : IRequestHandler<CreateMilestoneCommand, Guid>
{
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IContractRepository _contractRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public CreateMilestoneCommandHandler(
        IMilestoneRepository milestoneRepository,
        IContractRepository contractRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
    {
        _milestoneRepository = milestoneRepository;
        _contractRepository = contractRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateMilestoneCommand request, CancellationToken cancellationToken)
    {
        var contract = await _contractRepository.GetByIdAsync(request.ContractId, cancellationToken)
            ?? throw new NotFoundException(nameof(Contract), request.ContractId);

        // ProjectId/TenantId always derived from the loaded contract, never
        // trusted from the request — same defense CreateDecisionCommandHandler
        // uses for MeetingId-derived scoping.
        var milestone = Milestone.Create(contract.TenantId, contract.ProjectId, contract.Id, request.Title, _currentUser.UserId);

        await _milestoneRepository.AddAsync(milestone, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return milestone.Id;
    }
}
