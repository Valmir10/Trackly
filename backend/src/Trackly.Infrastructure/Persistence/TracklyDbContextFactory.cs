using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Infrastructure.Persistence;

internal sealed class DesignTimeCurrentTenantService : ICurrentTenantService
{
    public Guid TenantId => Guid.Empty;
}

public sealed class TracklyDbContextFactory : IDesignTimeDbContextFactory<TracklyDbContext>
{
    public TracklyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TracklyDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=trackly;Username=trackly;Password=trackly_dev_password");

        return new TracklyDbContext(optionsBuilder.Options, new DesignTimeCurrentTenantService());
    }
}
