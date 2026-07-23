namespace Trackly.Application.Features.Auth.Common;

public sealed record AuthResult(Guid UserId, Guid TenantId, string AccessToken, string RefreshToken);
