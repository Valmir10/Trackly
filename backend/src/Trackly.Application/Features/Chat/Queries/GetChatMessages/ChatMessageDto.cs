namespace Trackly.Application.Features.Chat.Queries.GetChatMessages;

public sealed record ChatMessageDto(
    Guid Id,
    Guid AuthorId,
    string AuthorInitials,
    string Content,
    Guid? TicketId,
    DateTime CreatedAt);
