using System.IdentityModel.Tokens.Jwt;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Api.Auth;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // Read lazily — see CurrentTenantService for why this can't be cached
    // at construction time.
    public Guid UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            return claim is not null && Guid.TryParse(claim.Value, out var userId) ? userId : Guid.Empty;
        }
    }
}
