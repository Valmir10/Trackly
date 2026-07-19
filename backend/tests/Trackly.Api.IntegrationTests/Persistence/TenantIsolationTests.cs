using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Api.IntegrationTests.Persistence;

file sealed class FakeCurrentTenantService : ICurrentTenantService
{
    public required Guid TenantId { get; init; }
}

public sealed class TenantIsolationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine").Build();

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        await using var migrationContext = CreateContext(Guid.Empty);
        await migrationContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
    }

    private TracklyDbContext CreateContext(Guid tenantId)
    {
        var options = new DbContextOptionsBuilder<TracklyDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        return new TracklyDbContext(options, new FakeCurrentTenantService { TenantId = tenantId });
    }

    // Tenant is excluded from the query filter, so it's reachable regardless
    // of which tenant context the DbContext is scoped to — but Project's
    // real FK constraint still means one has to exist before a Project can
    // reference it.
    private async Task<Guid> SeedTenantAsync()
    {
        await using var context = CreateContext(Guid.Empty);
        var tenant = Tenant.Create($"Tenant {Guid.NewGuid():N}", $"tenant-{Guid.NewGuid():N}");
        await context.Tenants.AddAsync(tenant);
        await context.SaveChangesAsync();
        return tenant.Id;
    }

    // -------------------------------------------------------
    // Cross-tenant isolation
    // -------------------------------------------------------

    [Fact]
    public async Task Project_CreatedUnderOneTenant_IsInvisibleToAnotherTenant()
    {
        // Arrange
        var tenantAId = await SeedTenantAsync();
        var tenantBId = await SeedTenantAsync();

        await using (var seedContext = CreateContext(tenantAId))
        {
            var project = Project.Create(tenantAId, "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());
            await seedContext.Projects.AddAsync(project);
            await seedContext.SaveChangesAsync();
        }

        // Act
        await using var tenantBContext = CreateContext(tenantBId);
        var visibleProjects = await tenantBContext.Projects.ToListAsync();

        // Assert
        visibleProjects.Should().BeEmpty();
    }

    // -------------------------------------------------------
    // Same-tenant access still works
    // -------------------------------------------------------

    [Fact]
    public async Task Project_IsVisibleWithinItsOwnTenant()
    {
        // Arrange
        var tenantId = await SeedTenantAsync();
        Guid projectId;

        await using (var seedContext = CreateContext(tenantId))
        {
            var project = Project.Create(tenantId, "Frontend redesign", "var(--tp-cat-1)", Guid.NewGuid());
            projectId = project.Id;
            await seedContext.Projects.AddAsync(project);
            await seedContext.SaveChangesAsync();
        }

        // Act
        await using var ownTenantContext = CreateContext(tenantId);
        var found = await ownTenantContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

        // Assert
        found.Should().NotBeNull();
    }
}
