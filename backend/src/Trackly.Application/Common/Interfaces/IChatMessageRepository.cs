using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message, CancellationToken cancellationToken);
    Task<IReadOnlyList<ChatMessage>> GetByScopeAsync(Guid projectId, Guid? ticketId, CancellationToken cancellationToken);
}
