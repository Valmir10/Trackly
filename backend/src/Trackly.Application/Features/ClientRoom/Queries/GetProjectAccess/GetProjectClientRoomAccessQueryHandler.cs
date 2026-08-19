using MediatR;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Application.Features.ClientRoom.Queries.GetProjectAccess;

public sealed class GetProjectClientRoomAccessQueryHandler : IRequestHandler<GetProjectClientRoomAccessQuery, IReadOnlyList<ClientRoomAccessDto>>
{
    private readonly IClientRoomAccessRepository _accessRepository;

    public GetProjectClientRoomAccessQueryHandler(IClientRoomAccessRepository accessRepository)
    {
        _accessRepository = accessRepository;
    }

    public async Task<IReadOnlyList<ClientRoomAccessDto>> Handle(GetProjectClientRoomAccessQuery request, CancellationToken cancellationToken)
    {
        var accessGrants = await _accessRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);

        return accessGrants
            .Select(a => new ClientRoomAccessDto(a.Id, a.ExpiresAt, a.CreatedAt, a.RevokedAt, a.IsActive))
            .ToList();
    }
}
