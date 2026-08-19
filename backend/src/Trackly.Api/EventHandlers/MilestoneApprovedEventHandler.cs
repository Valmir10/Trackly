using MediatR;
using Microsoft.AspNetCore.SignalR;
using Trackly.Api.Hubs;
using Trackly.Application.Common.Events;
using Trackly.Domain.Events;

namespace Trackly.Api.EventHandlers;

public sealed class MilestoneApprovedEventHandler : INotificationHandler<DomainEventNotification<MilestoneApprovedEvent>>
{
    private readonly IHubContext<ProjectHub> _hubContext;

    public MilestoneApprovedEventHandler(IHubContext<ProjectHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task Handle(DomainEventNotification<MilestoneApprovedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        return _hubContext.Clients.Group(ProjectHub.GroupName(domainEvent.ProjectId.ToString())).SendAsync(
            "MilestoneApproved",
            new
            {
                domainEvent.ApprovalId,
                domainEvent.MilestoneId,
                domainEvent.ProjectId,
                domainEvent.ApprovedAt,
            },
            cancellationToken);
    }
}
