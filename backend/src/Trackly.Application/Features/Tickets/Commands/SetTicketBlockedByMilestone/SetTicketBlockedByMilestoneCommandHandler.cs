using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByMilestone;

public sealed class SetTicketBlockedByMilestoneCommandHandler : IRequestHandler<SetTicketBlockedByMilestoneCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetTicketBlockedByMilestoneCommandHandler(
        ITicketRepository ticketRepository,
        IMilestoneRepository milestoneRepository,
        IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _milestoneRepository = milestoneRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SetTicketBlockedByMilestoneCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        if (request.MilestoneId is null)
        {
            ticket.ClearMilestoneBlock();
        }
        else
        {
            var milestone = await _milestoneRepository.GetByIdAsync(request.MilestoneId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Milestone), request.MilestoneId.Value);

            if (milestone.ProjectId != ticket.ProjectId)
                throw new ConflictException("Milestone belongs to a different project than the ticket.");

            ticket.SetBlockedByMilestone(milestone.Id);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
