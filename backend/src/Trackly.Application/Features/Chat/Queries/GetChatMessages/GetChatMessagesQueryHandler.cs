using MediatR;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Application.Features.Chat.Queries.GetChatMessages;

public sealed class GetChatMessagesQueryHandler : IRequestHandler<GetChatMessagesQuery, IReadOnlyList<ChatMessageDto>>
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IUserRepository _userRepository;

    public GetChatMessagesQueryHandler(IChatMessageRepository chatMessageRepository, IUserRepository userRepository)
    {
        _chatMessageRepository = chatMessageRepository;
        _userRepository = userRepository;
    }

    public async Task<IReadOnlyList<ChatMessageDto>> Handle(GetChatMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = await _chatMessageRepository.GetByScopeAsync(request.ProjectId, request.TicketId, cancellationToken);

        var initialsByUserId = new Dictionary<Guid, string>();
        var results = new List<ChatMessageDto>(messages.Count);

        foreach (var message in messages)
        {
            if (!initialsByUserId.TryGetValue(message.AuthorId, out var initials))
            {
                var user = await _userRepository.GetByIdAsync(message.AuthorId, cancellationToken);
                initials = user is null ? "?" : $"{user.FirstName[0]}{user.LastName[0]}".ToUpperInvariant();
                initialsByUserId[message.AuthorId] = initials;
            }

            results.Add(new ChatMessageDto(
                message.Id,
                message.AuthorId,
                initials,
                message.Content,
                message.TicketId,
                message.CreatedAt));
        }

        return results;
    }
}
