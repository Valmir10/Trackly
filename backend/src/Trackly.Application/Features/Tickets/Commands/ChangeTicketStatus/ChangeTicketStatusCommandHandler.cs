using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;

namespace Trackly.Application.Features.Tickets.Commands.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandHandler : IRequestHandler<ChangeTicketStatusCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeTicketStatusCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeTicketStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        ticket.ChangeStatus(request.NewStatus, request.Position);

        // Cascade inline, before the single SaveChangesAsync call below —
        // one-directional: moving back out of Done never re-blocks
        // downstream tickets that were already cleared.
        if (request.NewStatus == TicketStatus.Done)
        {
            var blockedTickets = await _ticketRepository.GetBlockedByTicketAsync(ticket.Id, cancellationToken);
            foreach (var blockedTicket in blockedTickets)
            {
                blockedTicket.ClearTicketBlock();
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
