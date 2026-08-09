using MediatR;
using Microsoft.AspNetCore.SignalR;
using Trackly.Api.Hubs;
using Trackly.Application.Common.Events;
using Trackly.Domain.Enums;
using Trackly.Domain.Events;

namespace Trackly.Api.EventHandlers;

public sealed class TicketCreatedEventHandler : INotificationHandler<DomainEventNotification<TicketCreatedEvent>>
{
    private readonly IHubContext<ProjectHub> _hubContext;

    public TicketCreatedEventHandler(IHubContext<ProjectHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Handle(DomainEventNotification<TicketCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // A newly created ticket always starts in ToDo — this is a domain
        // invariant enforced by Ticket.Create, not something that needs to
        // travel on the event itself.
        return _hubContext.Clients.Group(ProjectHub.GroupName(domainEvent.ProjectId.ToString())).SendAsync(
            "TicketCreated",
            new
            {
                domainEvent.TicketId,
                domainEvent.ProjectId,
                domainEvent.Title,
                Status = TicketStatus.ToDo.ToString(),
            },
            cancellationToken);
    }
}
