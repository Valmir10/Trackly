using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Infrastructure.Persistence;

internal sealed class DesignTimeCurrentTenantService : ICurrentTenantService
{
    public Guid TenantId => Guid.Empty;
}

// Migrations never raise domain events, so a no-op publisher is enough here.
internal sealed class DesignTimePublisher : IPublisher
{
    public Task Publish(object notification, CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification => Task.CompletedTask;
}

public sealed class TracklyDbContextFactory : IDesignTimeDbContextFactory<TracklyDbContext>
{
    public TracklyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TracklyDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=trackly;Username=trackly;Password=trackly_dev_password");

        return new TracklyDbContext(optionsBuilder.Options, new DesignTimeCurrentTenantService(), new DesignTimePublisher());
    }
}
