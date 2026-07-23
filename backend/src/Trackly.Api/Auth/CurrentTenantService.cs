using Trackly.Application.Common.Interfaces;

namespace Trackly.Api.Auth;

public sealed class CurrentTenantService : ICurrentTenantService
{
    public Guid TenantId { get; }

    public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
    {
        var claim = httpContextAccessor.HttpContext?.User.FindFirst("tenant_id");
        TenantId = claim is not null && Guid.TryParse(claim.Value, out var tenantId) ? tenantId : Guid.Empty;
    }
}
