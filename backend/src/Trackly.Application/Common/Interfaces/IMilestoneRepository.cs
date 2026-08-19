using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IMilestoneRepository
{
    Task AddAsync(Milestone milestone, CancellationToken cancellationToken);
    Task<Milestone?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // Returns null if the milestone doesn't exist OR belongs to a
    // different project — the enforcement point for the cross-project
    // guard, so callers get a single line instead of repeating the check.
    Task<Milestone?> GetByIdForProjectAsync(Guid id, Guid projectId, CancellationToken cancellationToken);

    Task<IReadOnlyList<Milestone>> GetByContractIdAsync(Guid contractId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Milestone>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Milestone>> GetAllAsync(CancellationToken cancellationToken);
}
