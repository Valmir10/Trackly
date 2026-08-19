using MediatR;

namespace Trackly.Application.Features.ClientRoom.Commands.CreateAccess;

public sealed record CreateClientRoomAccessCommand(Guid ProjectId) : IRequest<CreateClientRoomAccessResult>;

// RawToken is present ONLY in this response — never persisted raw, never
// retrievable again after this call returns.
public sealed record CreateClientRoomAccessResult(Guid AccessId, string RawToken);
