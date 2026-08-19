using MediatR;

namespace Trackly.Application.Features.ClientRoom.Commands.RevokeAccess;

public sealed record RevokeClientRoomAccessCommand(Guid AccessId) : IRequest;
