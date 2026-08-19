using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Tickets.Commands.AssignTicketToMilestone;

public sealed class AssignTicketToMilestoneCommandHandler : IRequestHandler<AssignTicketToMilestoneCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignTicketToMilestoneCommandHandler(
        ITicketRepository ticketRepository,
        IMilestoneRepository milestoneRepository,
        IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _milestoneRepository = milestoneRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AssignTicketToMilestoneCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        if (request.MilestoneId is null)
        {
            ticket.RemoveFromMilestone();
        }
        else
        {
            var milestone = await _milestoneRepository.GetByIdAsync(request.MilestoneId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Milestone), request.MilestoneId.Value);

            // The existing tenant-only query filters don't stop an internal
            // user from wiring a ticket to a milestone in a different
            // project within the same tenant — guard it explicitly here.
            if (milestone.ProjectId != ticket.ProjectId)
                throw new ConflictException("Milestone belongs to a different project than the ticket.");

            ticket.AssignToMilestone(milestone.Id);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
