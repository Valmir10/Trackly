using Trackly.Application.Common.Interfaces;

namespace Trackly.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TracklyDbContext _context;

    public UnitOfWork(TracklyDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
