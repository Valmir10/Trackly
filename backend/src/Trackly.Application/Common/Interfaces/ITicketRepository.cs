using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Ticket>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Ticket>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<Ticket>> GetBlockedByTicketAsync(Guid ticketId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Ticket>> GetBlockedByMilestoneAsync(Guid milestoneId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Ticket>> GetByMilestoneIdAsync(Guid milestoneId, CancellationToken cancellationToken);
}
