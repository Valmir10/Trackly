using MediatR;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

namespace Trackly.Application.Features.Tickets.Queries.GetWorkspaceTickets;

public sealed class GetWorkspaceTicketsQueryHandler : IRequestHandler<GetWorkspaceTicketsQuery, IReadOnlyList<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public GetWorkspaceTicketsQueryHandler(ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<TicketDto>> Handle(GetWorkspaceTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.GetAllAsync(cancellationToken);

        var initialsByUserId = new Dictionary<Guid, string?>();
        var results = new List<TicketDto>(tickets.Count);

        foreach (var ticket in tickets)
        {
            string? initials = null;
            if (ticket.AssignedToId is Guid assignedToId)
            {
                if (!initialsByUserId.TryGetValue(assignedToId, out initials))
                {
                    var user = await _userRepository.GetByIdAsync(assignedToId, cancellationToken);
                    initials = user is null ? null : $"{user.FirstName[0]}{user.LastName[0]}".ToUpperInvariant();
                    initialsByUserId[assignedToId] = initials;
                }
            }

            results.Add(new TicketDto(
                ticket.Id,
                ticket.ProjectId,
                ticket.Title,
                ticket.Description,
                ticket.Status,
                ticket.Priority,
                ticket.AssignedToId,
                initials,
                ticket.DueDate,
                ticket.Position,
                ticket.CreatedAt,
                ticket.CompletedAt));
        }

        return results;
    }
}
