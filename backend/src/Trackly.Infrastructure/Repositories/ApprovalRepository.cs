using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class ApprovalRepository : IApprovalRepository
{
    private readonly TracklyDbContext _context;

    public ApprovalRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Approval approval, CancellationToken cancellationToken)
    {
        await _context.Approvals.AddAsync(approval, cancellationToken);
    }

    public async Task<Approval?> GetByMilestoneIdAsync(Guid milestoneId, CancellationToken cancellationToken)
    {
        return await _context.Approvals.FirstOrDefaultAsync(a => a.MilestoneId == milestoneId, cancellationToken);
    }
}
