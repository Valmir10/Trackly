using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class ClientRoomAccessRepository : IClientRoomAccessRepository
{
    private readonly TracklyDbContext _context;

    public ClientRoomAccessRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ClientRoomAccess access, CancellationToken cancellationToken)
    {
        await _context.ClientRoomAccesses.AddAsync(access, cancellationToken);
    }

    public async Task<ClientRoomAccess?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.ClientRoomAccesses.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<ClientRoomAccess?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken)
    {
        return await _context.ClientRoomAccesses.FirstOrDefaultAsync(a => a.TokenHash == tokenHash, cancellationToken);
    }

    public async Task<IReadOnlyList<ClientRoomAccess>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.ClientRoomAccesses
            .Where(a => a.ProjectId == projectId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
