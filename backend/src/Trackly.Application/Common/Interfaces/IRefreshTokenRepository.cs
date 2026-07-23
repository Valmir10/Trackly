using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken);
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);
}
