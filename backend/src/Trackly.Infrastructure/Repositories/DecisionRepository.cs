using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class DecisionRepository : IDecisionRepository
{
    private readonly TracklyDbContext _context;

    public DecisionRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Decision decision, CancellationToken cancellationToken)
    {
        await _context.Decisions.AddAsync(decision, cancellationToken);
    }

    public async Task<IReadOnlyList<Decision>> GetByMeetingIdAsync(Guid meetingId, CancellationToken cancellationToken)
    {
        return await _context.Decisions
            .Where(d => d.MeetingId == meetingId)
            .OrderBy(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Decision>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Decisions.OrderBy(d => d.CreatedAt).ToListAsync(cancellationToken);
    }
}
