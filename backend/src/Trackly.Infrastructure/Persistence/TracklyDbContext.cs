using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence;

public sealed class TracklyDbContext : DbContext
{
    private readonly ICurrentTenantService _currentTenantService;

    public TracklyDbContext(DbContextOptions<TracklyDbContext> options, ICurrentTenantService currentTenantService)
        : base(options)
    {
        _currentTenantService = currentTenantService;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TracklyDbContext).Assembly);

        // Global query filters: the actual multi-tenancy enforcement point.
        // Deliberately not applied to User — login must be able to look a
        // user up by email before any tenant context exists.
        modelBuilder.Entity<Project>().HasQueryFilter(p => p.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<Ticket>().HasQueryFilter(t => t.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<ChatMessage>().HasQueryFilter(m => m.TenantId == _currentTenantService.TenantId);

        base.OnModelCreating(modelBuilder);
    }
}
