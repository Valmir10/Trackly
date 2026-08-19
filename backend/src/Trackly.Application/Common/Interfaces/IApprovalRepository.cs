using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IApprovalRepository
{
    Task AddAsync(Approval approval, CancellationToken cancellationToken);
    Task<Approval?> GetByMilestoneIdAsync(Guid milestoneId, CancellationToken cancellationToken);
}
