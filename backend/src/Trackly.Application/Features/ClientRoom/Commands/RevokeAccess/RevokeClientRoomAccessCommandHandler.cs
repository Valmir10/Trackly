using MediatR;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Application.Features.ClientRoom.Commands.RevokeAccess;

public sealed class RevokeClientRoomAccessCommandHandler : IRequestHandler<RevokeClientRoomAccessCommand>
{
    private readonly IClientRoomAccessRepository _accessRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevokeClientRoomAccessCommandHandler(IClientRoomAccessRepository accessRepository, IUnitOfWork unitOfWork)
    {
        _accessRepository = accessRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RevokeClientRoomAccessCommand request, CancellationToken cancellationToken)
    {
        var access = await _accessRepository.GetByIdAsync(request.AccessId, cancellationToken)
            ?? throw new NotFoundException(nameof(ClientRoomAccess), request.AccessId);

        access.Revoke();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
