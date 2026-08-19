using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByTicket;

public sealed class SetTicketBlockedByTicketCommandHandler : IRequestHandler<SetTicketBlockedByTicketCommand>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetTicketBlockedByTicketCommandHandler(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SetTicketBlockedByTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        if (request.BlockingTicketId is null)
        {
            ticket.ClearTicketBlock();
        }
        else
        {
            var blockingTicket = await _ticketRepository.GetByIdAsync(request.BlockingTicketId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Ticket), request.BlockingTicketId.Value);

            if (blockingTicket.ProjectId != ticket.ProjectId)
                throw new ConflictException("Blocking ticket belongs to a different project.");

            ticket.SetBlockedByTicket(blockingTicket.Id);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
