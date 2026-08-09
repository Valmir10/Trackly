using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class TicketRepository : ITicketRepository
{
    private readonly TracklyDbContext _context;

    public TicketRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        await _context.Tickets.AddAsync(ticket, cancellationToken);
    }

    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Ticket>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _context.Tickets
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.Position)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Tickets.ToListAsync(cancellationToken);
    }
}
