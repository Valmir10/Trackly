using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenantService _currentTenant;
    private readonly ICurrentUserService _currentUser;

    public CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentTenantService currentTenant,
        ICurrentUserService currentUser)
    {
        _ticketRepository = ticketRepository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Project), request.ProjectId);

        var ticket = Ticket.Create(
            _currentTenant.TenantId,
            project.Id,
            request.Title,
            _currentUser.UserId,
            request.Priority,
            request.AssignedToId,
            request.DueDate,
            request.Description);

        await _ticketRepository.AddAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}
