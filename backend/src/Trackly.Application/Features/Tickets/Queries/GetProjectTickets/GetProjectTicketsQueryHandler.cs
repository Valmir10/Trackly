using MediatR;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

public sealed class GetProjectTicketsQueryHandler : IRequestHandler<GetProjectTicketsQuery, IReadOnlyList<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public GetProjectTicketsQueryHandler(ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<TicketDto>> Handle(GetProjectTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        // Small, deliberate N+1: at this project's realistic ticket-count
        // scale, a batch-lookup repository method would be premature —
        // revisit if a real project ever has enough assignees for it to
        // matter.
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
                ticket.CompletedAt,
                ticket.UpdatedAt,
                ticket.MilestoneId,
                ticket.BlockedByTicketId,
                ticket.BlockedByMilestoneId));
        }

        return results;
    }
}
