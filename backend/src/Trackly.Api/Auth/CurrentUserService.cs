using System.IdentityModel.Tokens.Jwt;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Api.Auth;

public sealed class CurrentUserService : ICurrentUserService
{
    public Guid UserId { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var claim = httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
        UserId = claim is not null && Guid.TryParse(claim.Value, out var userId) ? userId : Guid.Empty;
    }
}
