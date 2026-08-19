using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class MilestoneRepository : IMilestoneRepository
{
    private readonly TracklyDbContext _context;

    public MilestoneRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Milestone milestone, CancellationToken cancellationToken)
    {
        await _context.Milestones.AddAsync(milestone, cancellationToken);
    }

    public async Task<Milestone?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Milestones.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Milestone?> GetByIdForProjectAsync(Guid id, Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Milestones.FirstOrDefaultAsync(m => m.Id == id && m.ProjectId == projectId, cancellationToken);
    }

    public async Task<IReadOnlyList<Milestone>> GetByContractIdAsync(Guid contractId, CancellationToken cancellationToken)
    {
        return await _context.Milestones
            .Where(m => m.ContractId == contractId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Milestone>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Milestones
            .Where(m => m.ProjectId == projectId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
