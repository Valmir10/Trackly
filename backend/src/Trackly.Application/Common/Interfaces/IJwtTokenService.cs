namespace Trackly.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, Guid tenantId, string email, string role);

    (string RawToken, string Hash) GenerateRefreshToken();

    string HashRefreshToken(string rawToken);
}
