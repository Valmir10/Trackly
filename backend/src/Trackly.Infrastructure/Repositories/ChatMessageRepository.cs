using Microsoft.EntityFrameworkCore;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;
using Trackly.Infrastructure.Persistence;

namespace Trackly.Infrastructure.Repositories;

public sealed class ChatMessageRepository : IChatMessageRepository
{
    private readonly TracklyDbContext _context;

    public ChatMessageRepository(TracklyDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        await _context.ChatMessages.AddAsync(message, cancellationToken);
    }

    public async Task<IReadOnlyList<ChatMessage>> GetByScopeAsync(Guid projectId, Guid? ticketId, CancellationToken cancellationToken)
    {
        return await _context.ChatMessages
            .Where(m => m.ProjectId == projectId && m.TicketId == ticketId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
