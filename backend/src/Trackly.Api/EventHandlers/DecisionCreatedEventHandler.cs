using MediatR;
using Microsoft.AspNetCore.SignalR;
using Trackly.Api.Hubs;
using Trackly.Application.Common.Events;
using Trackly.Domain.Events;

namespace Trackly.Api.EventHandlers;

public sealed class DecisionCreatedEventHandler : INotificationHandler<DomainEventNotification<DecisionCreatedEvent>>
{
    private readonly IHubContext<ProjectHub> _hubContext;

    public DecisionCreatedEventHandler(IHubContext<ProjectHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Handle(DomainEventNotification<DecisionCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        return _hubContext.Clients.Group(ProjectHub.GroupName(domainEvent.ProjectId.ToString())).SendAsync(
            "DecisionCreated",
            new
            {
                domainEvent.DecisionId,
                domainEvent.MeetingId,
                domainEvent.ProjectId,
                domainEvent.Text,
                domainEvent.CreatedById,
                domainEvent.CreatedAt,
            },
            cancellationToken);
    }
}
