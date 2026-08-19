using MediatR;

namespace Trackly.Application.Features.ClientRoom.Queries.GetProjectAccess;

public sealed record GetProjectClientRoomAccessQuery(Guid ProjectId) : IRequest<IReadOnlyList<ClientRoomAccessDto>>;
