using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IDecisionRepository
{
    Task AddAsync(Decision decision, CancellationToken cancellationToken);
    Task<IReadOnlyList<Decision>> GetByMeetingIdAsync(Guid meetingId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Decision>> GetAllAsync(CancellationToken cancellationToken);
}
