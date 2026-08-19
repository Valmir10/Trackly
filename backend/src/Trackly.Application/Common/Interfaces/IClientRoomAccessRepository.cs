using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IClientRoomAccessRepository
{
    Task AddAsync(ClientRoomAccess access, CancellationToken cancellationToken);
    Task<ClientRoomAccess?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ClientRoomAccess?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);
    Task<IReadOnlyList<ClientRoomAccess>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
}
