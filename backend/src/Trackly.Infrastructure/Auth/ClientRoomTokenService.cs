using System.Security.Cryptography;
using System.Text;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Infrastructure.Auth;

// Same random-bytes + SHA256 primitive as JwtTokenService's refresh-token
// generation, kept as its own service since a client-room token is a
// conceptually distinct credential.
public sealed class ClientRoomTokenService : IClientRoomTokenService
{
    public (string RawToken, string Hash) GenerateToken()
    {
        var rawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return (rawToken, Hash(rawToken));
    }

    public string Hash(string rawToken)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
        return Convert.ToHexString(hashBytes);
    }
}
