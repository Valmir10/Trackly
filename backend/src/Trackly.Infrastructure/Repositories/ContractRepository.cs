using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class ContractRepository : IContractRepository
{
    private readonly TracklyDbContext _context;

    public ContractRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Contract contract, CancellationToken cancellationToken)
    {
        await _context.Contracts.AddAsync(contract, cancellationToken);
    }

    public async Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Contracts.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Contract>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Contracts
            .Where(c => c.ProjectId == projectId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
