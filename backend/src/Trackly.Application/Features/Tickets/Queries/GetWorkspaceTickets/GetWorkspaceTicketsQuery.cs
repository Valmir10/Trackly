using MediatR;
using Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

namespace Trackly.Application.Features.Tickets.Queries.GetWorkspaceTickets;

public sealed record GetWorkspaceTicketsQuery : IRequest<IReadOnlyList<TicketDto>>;
