using MediatR;
using Microsoft.AspNetCore.SignalR;
using Trackly.Api.Hubs;
using Trackly.Application.Common.Events;
using Trackly.Domain.Events;

namespace Trackly.Api.EventHandlers;

public sealed class ChatMessageSentEventHandler : INotificationHandler<DomainEventNotification<ChatMessageSentEvent>>
{
    private readonly IHubContext<ProjectHub> _hubContext;

    public ChatMessageSentEventHandler(IHubContext<ProjectHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Handle(DomainEventNotification<ChatMessageSentEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        return _hubContext.Clients.Group(ProjectHub.GroupName(domainEvent.ProjectId.ToString())).SendAsync(
            "ChatMessageSent",
            new { domainEvent.MessageId, domainEvent.TicketId, domainEvent.AuthorId },
            cancellationToken);
    }
}
