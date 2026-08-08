using MediatR;
using Microsoft.AspNetCore.SignalR;
using Trackly.Api.Hubs;
using Trackly.Application.Common.Events;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Events;

namespace Trackly.Api.EventHandlers;

public sealed class ChatMessageSentEventHandler : INotificationHandler<DomainEventNotification<ChatMessageSentEvent>>
{
    private readonly IHubContext<ProjectHub> _hubContext;
    private readonly IUserRepository _userRepository;

    public ChatMessageSentEventHandler(IHubContext<ProjectHub> hubContext, IUserRepository userRepository)
    {
        _hubContext = hubContext;
        _userRepository = userRepository;
    }

    public async Task Handle(DomainEventNotification<ChatMessageSentEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // The broadcast carries the full message, not just ids — the
        // frontend appends it directly on receipt instead of re-fetching.
        var author = await _userRepository.GetByIdAsync(domainEvent.AuthorId, cancellationToken);
        var authorInitials = author is null ? "?" : $"{author.FirstName[0]}{author.LastName[0]}".ToUpperInvariant();

        await _hubContext.Clients.Group(ProjectHub.GroupName(domainEvent.ProjectId.ToString())).SendAsync(
            "ChatMessageSent",
            new
            {
                domainEvent.MessageId,
                domainEvent.TicketId,
                domainEvent.AuthorId,
                AuthorInitials = authorInitials,
                domainEvent.Content,
                domainEvent.CreatedAt,
            },
            cancellationToken);
    }
}
