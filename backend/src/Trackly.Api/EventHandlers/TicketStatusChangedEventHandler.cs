using MediatR;
using Microsoft.AspNetCore.SignalR;
using Trackly.Api.Hubs;
using Trackly.Application.Common.Events;
using Trackly.Domain.Events;

namespace Trackly.Api.EventHandlers;

public sealed class TicketStatusChangedEventHandler : INotificationHandler<DomainEventNotification<TicketStatusChangedEvent>>
{
    private readonly IHubContext<ProjectHub> _hubContext;

    public TicketStatusChangedEventHandler(IHubContext<ProjectHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Handle(DomainEventNotification<TicketStatusChangedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        return _hubContext.Clients.Group(ProjectHub.GroupName(domainEvent.ProjectId.ToString())).SendAsync(
            "TicketStatusChanged",
            new { domainEvent.TicketId, Status = domainEvent.NewStatus.ToString() },
            cancellationToken);
    }
}
