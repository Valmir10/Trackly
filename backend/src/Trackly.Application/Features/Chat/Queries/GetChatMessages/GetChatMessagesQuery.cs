using MediatR;

namespace Trackly.Application.Features.Chat.Queries.GetChatMessages;

public sealed record GetChatMessagesQuery(Guid ProjectId, Guid? TicketId) : IRequest<IReadOnlyList<ChatMessageDto>>;
