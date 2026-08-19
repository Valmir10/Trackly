namespace Trackly.Application.Common.Interfaces;

// Mirrors IJwtTokenService's GenerateRefreshToken/HashRefreshToken shape —
// same random-bytes + SHA256 primitive, kept as its own service since a
// client-room token is a conceptually distinct credential (never rotated,
// never exchanged), not a refresh token.
public interface IClientRoomTokenService
{
    (string RawToken, string Hash) GenerateToken();
    string Hash(string rawToken);
}
