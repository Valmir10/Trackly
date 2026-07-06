using Trackly.Domain.Entities;

namespace Trackly.Application.Common.Interfaces;

public interface IChatMessageRepository
{
    Task AddAsync(ChatMessage message, CancellationToken cancellationToken);
}
