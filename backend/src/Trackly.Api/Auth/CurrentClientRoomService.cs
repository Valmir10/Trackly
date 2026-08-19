using Trackly.Application.Common.Interfaces;

namespace Trackly.Api.Auth;

public sealed class CurrentClientRoomService : ICurrentClientRoomService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentClientRoomService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Read lazily — see CurrentTenantService for why this can't be cached
    // at construction time.
    public Guid AccessId => ReadGuidClaim("client_room_access_id");
    public Guid ProjectId => ReadGuidClaim("project_id");

    private Guid ReadGuidClaim(string claimType)
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst(claimType);
        return claim is not null && Guid.TryParse(claim.Value, out var value) ? value : Guid.Empty;
    }
}
