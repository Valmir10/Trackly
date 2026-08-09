using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IMeetingRepository
{
    Task AddAsync(Meeting meeting, CancellationToken cancellationToken);
    Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Meeting>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Meeting>> GetAllAsync(CancellationToken cancellationToken);
}
