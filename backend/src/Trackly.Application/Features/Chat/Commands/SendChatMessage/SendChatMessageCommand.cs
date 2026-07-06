using MediatR;

namespace Trackly.Application.Features.Chat.Commands.SendChatMessage;

public sealed record SendChatMessageCommand(Guid ProjectId, Guid? TicketId, string Content) : IRequest<Guid>;
