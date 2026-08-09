using MediatR;
using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Events;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Common;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence;

public sealed class TracklyDbContext : DbContext
{
    private readonly ICurrentTenantService _currentTenantService;
    private readonly IPublisher _publisher;

    public TracklyDbContext(DbContextOptions<TracklyDbContext> options, ICurrentTenantService currentTenantService, IPublisher publisher)
        : base(options)
    {
        _currentTenantService = currentTenantService;
        _publisher = publisher;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<Decision> Decisions => Set<Decision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TracklyDbContext).Assembly);

        // Global query filters: the actual multi-tenancy enforcement point.
        // Deliberately not applied to User — login must be able to look a
        // user up by email before any tenant context exists.
        modelBuilder.Entity<Project>().HasQueryFilter(p => p.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<Ticket>().HasQueryFilter(t => t.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<ChatMessage>().HasQueryFilter(m => m.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<Meeting>().HasQueryFilter(m => m.TenantId == _currentTenantService.TenantId);
        modelBuilder.Entity<Decision>().HasQueryFilter(d => d.TenantId == _currentTenantService.TenantId);

        base.OnModelCreating(modelBuilder);
    }

    // Domain events sit on each AggregateRoot until something actually
    // dispatches them — this is that dispatch point. Collected before the
    // save (so we still have the pre-clear list), published only after the
    // save succeeds (so nothing fires for a change that never committed),
    // then cleared so the same event can't be published twice.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregatesWithEvents = ChangeTracker.Entries<AggregateRoot>()
            .Select(entry => entry.Entity)
            .Where(aggregate => aggregate.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = aggregatesWithEvents.SelectMany(aggregate => aggregate.DomainEvents).ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;
            await _publisher.Publish(notification, cancellationToken);
        }

        foreach (var aggregate in aggregatesWithEvents)
        {
            aggregate.ClearDomainEvents();
        }

        return result;
    }
}
