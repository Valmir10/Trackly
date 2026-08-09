using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class MeetingRepository : IMeetingRepository
{
    private readonly TracklyDbContext _context;

    public MeetingRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Meeting meeting, CancellationToken cancellationToken)
    {
        await _context.Meetings.AddAsync(meeting, cancellationToken);
    }

    public async Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Meetings.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }
}
