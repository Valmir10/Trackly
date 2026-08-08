using MediatR;

namespace Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

public sealed record GetProjectTicketsQuery(Guid ProjectId) : IRequest<IReadOnlyList<TicketDto>>;
