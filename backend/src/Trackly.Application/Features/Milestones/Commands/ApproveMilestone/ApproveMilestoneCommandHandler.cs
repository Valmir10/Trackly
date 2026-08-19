using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Milestones.Commands.ApproveMilestone;

public sealed class ApproveMilestoneCommandHandler : IRequestHandler<ApproveMilestoneCommand, Guid>
{
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IApprovalRepository _approvalRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ICurrentClientRoomService _currentClientRoom;

    public ApproveMilestoneCommandHandler(
        IMilestoneRepository milestoneRepository,
        IApprovalRepository approvalRepository,
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        ICurrentTenantService currentTenant,
        ICurrentClientRoomService currentClientRoom)
    {
        _milestoneRepository = milestoneRepository;
        _approvalRepository = approvalRepository;
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentClientRoom = currentClientRoom;
    }

    public async Task<Guid> Handle(ApproveMilestoneCommand request, CancellationToken cancellationToken)
    {
        // Scoped to the token's own project — a milestone from a different
        // project (even in the same tenant) is a 404, not a 403, so the
        // response never confirms whether it exists at all.
        var milestone = await _milestoneRepository.GetByIdForProjectAsync(request.MilestoneId, _currentClientRoom.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Milestone), request.MilestoneId);

        var existingApproval = await _approvalRepository.GetByMilestoneIdAsync(milestone.Id, cancellationToken);
        if (existingApproval is not null)
            throw new ConflictException("Milestone has already been approved.");

        var approval = Approval.Create(_currentTenant.TenantId, milestone.ProjectId, milestone.Id, _currentClientRoom.AccessId);
        await _approvalRepository.AddAsync(approval, cancellationToken);

        // Inline cascade — loads and mutates the downstream Ticket
        // aggregates directly here, before the single SaveChangesAsync call
        // below, rather than reacting to MilestoneApprovedEvent from a
        // separate handler (which would re-enter TracklyDbContext.SaveChangesAsync).
        var blockedTickets = await _ticketRepository.GetBlockedByMilestoneAsync(milestone.Id, cancellationToken);
        foreach (var ticket in blockedTickets)
        {
            ticket.ClearMilestoneBlock();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return approval.Id;
    }
}
