using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class TenantRepository : ITenantRepository
{
    private readonly TracklyDbContext _context;

    public TenantRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken)
    {
        await _context.Tenants.AddAsync(tenant, cancellationToken);
    }

    public async Task<Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }
}
