using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IContractRepository
{
    Task AddAsync(Contract contract, CancellationToken cancellationToken);
    Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Contract>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
}
