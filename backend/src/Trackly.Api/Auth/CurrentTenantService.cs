using Trackly.Application.Common.Interfaces;

namespace Trackly.Api.Auth;

public sealed class CurrentTenantService : ICurrentTenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Read lazily, not cached at construction — a scoped service can be
    // constructed (e.g. transitively, via TracklyDbContext) before
    // authentication/authorization has finished setting HttpContext.User,
    // as happens when ClientRoomAuthenticationHandler itself needs a
    // TracklyDbContext to validate the token. Reading fresh on every access
    // means whichever principal ends up on HttpContext.User by the time
    // this is actually used is the one that counts.
    public Guid TenantId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("tenant_id");
            return claim is not null && Guid.TryParse(claim.Value, out var tenantId) ? tenantId : Guid.Empty;
        }
    }
}
