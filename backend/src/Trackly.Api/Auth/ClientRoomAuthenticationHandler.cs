using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Api.Auth;

// Validates the raw magic-link token fresh against the DB on every request —
// no session, no caching, no exchange step. RevokedAt takes effect on the
// very next request. Deliberately issues no "sub" claim: CurrentUserService
// stays at Guid.Empty for a client-room request, so any handler mistakenly
// wired to use it for an audit FK hard-fails instead of writing garbage.
public sealed class ClientRoomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IClientRoomAccessRepository _accessRepository;
    private readonly IClientRoomTokenService _tokenService;

    public ClientRoomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IClientRoomAccessRepository accessRepository,
        IClientRoomTokenService tokenService)
        : base(options, logger, encoder)
    {
        _accessRepository = accessRepository;
        _tokenService = tokenService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var header = Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.NoResult();

        var rawToken = header["Bearer ".Length..].Trim();
        if (string.IsNullOrEmpty(rawToken))
            return AuthenticateResult.NoResult();

        var hash = _tokenService.Hash(rawToken);
        var access = await _accessRepository.GetByHashAsync(hash, Context.RequestAborted);

        if (access is null || !access.IsActive)
            return AuthenticateResult.Fail("Client room access token is invalid, expired, or revoked.");

        var claims = new[]
        {
            new Claim("client_room_access_id", access.Id.ToString()),
            new Claim("tenant_id", access.TenantId.ToString()),
            new Claim("project_id", access.ProjectId.ToString()),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
