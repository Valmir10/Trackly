using Microsoft.AspNetCore.SignalR;

namespace Trackly.Api.Hubs;

public sealed class ProjectHub : Hub
{
    public Task JoinProject(string projectId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, GroupName(projectId));
    }

    public Task LeaveProject(string projectId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(projectId));
    }

    public static string GroupName(string projectId) => $"project-{projectId}";
}
