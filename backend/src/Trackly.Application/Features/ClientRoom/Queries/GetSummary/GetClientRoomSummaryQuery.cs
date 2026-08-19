using MediatR;

namespace Trackly.Application.Features.ClientRoom.Queries.GetSummary;

// Zero params — everything derived from ICurrentClientRoomService.ProjectId
// inside the handler, never from client input.
public sealed record GetClientRoomSummaryQuery : IRequest<ClientRoomSummaryDto>;
