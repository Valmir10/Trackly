using Trackly.Domain.Common;
using Trackly.Domain.Events;

namespace Trackly.Domain.Entities;

public sealed class ChatMessage : AggregateRoot
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? TicketId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private ChatMessage() { }

    public static ChatMessage Create(Guid tenantId, Guid projectId, Guid authorId, string content, Guid? ticketId = null)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty.", nameof(tenantId));

        if (projectId == Guid.Empty)
            throw new ArgumentException("ProjectId cannot be empty.", nameof(projectId));

        if (authorId == Guid.Empty)
            throw new ArgumentException("AuthorId cannot be empty.", nameof(authorId));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Message content cannot be empty.", nameof(content));

        var message = new ChatMessage
        {
            TenantId = tenantId,
            ProjectId = projectId,
            TicketId = ticketId,
            AuthorId = authorId,
            Content = content.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        message.AddDomainEvent(new ChatMessageSentEvent(message.Id, projectId, ticketId, authorId, message.Content, message.CreatedAt));
        return message;
    }
}
