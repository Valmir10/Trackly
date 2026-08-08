using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Ticket>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
}
