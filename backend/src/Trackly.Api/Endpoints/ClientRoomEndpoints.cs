using MediatR;
using Trackly.Api.Auth;
using Trackly.Application.Features.ClientRoom.Queries.GetSummary;
using Trackly.Application.Features.Milestones.Commands.ApproveMilestone;

namespace Trackly.Api.Endpoints;

public static class ClientRoomEndpoints
{
    public static void MapClientRoomEndpoints(this WebApplication app)
    {
        app.MapGet("/api/client-room/summary", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var summary = await sender.Send(new GetClientRoomSummaryQuery(), cancellationToken);
            return Results.Ok(summary);
        }).RequireAuthorization(ClientRoomAuthDefaults.Scheme);

        app.MapPost("/api/client-room/milestones/{milestoneId:guid}/approve", async (
            Guid milestoneId, ISender sender, CancellationToken cancellationToken) =>
        {
            var approvalId = await sender.Send(new ApproveMilestoneCommand(milestoneId), cancellationToken);
            return Results.Ok(new { ApprovalId = approvalId });
        }).RequireAuthorization(ClientRoomAuthDefaults.Scheme);
    }
}
