using MediatR;
using Trackly.Application.Features.ClientRoom.Commands.CreateAccess;
using Trackly.Application.Features.ClientRoom.Commands.RevokeAccess;
using Trackly.Application.Features.ClientRoom.Queries.GetProjectAccess;

namespace Trackly.Api.Endpoints;

public static class ClientRoomAccessEndpoints
{
    public static void MapClientRoomAccessEndpoints(this WebApplication app)
    {
        app.MapPost("/api/projects/{projectId:guid}/client-room-access", async (
            Guid projectId, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new CreateClientRoomAccessCommand(projectId), cancellationToken);
            return Results.Created($"/api/client-room-access/{result.AccessId}", result);
        }).RequireAuthorization();

        app.MapPost("/api/client-room-access/{accessId:guid}/revoke", async (
            Guid accessId, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new RevokeClientRoomAccessCommand(accessId), cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization();

        app.MapGet("/api/projects/{projectId:guid}/client-room-access", async (
            Guid projectId, ISender sender, CancellationToken cancellationToken) =>
        {
            var access = await sender.Send(new GetProjectClientRoomAccessQuery(projectId), cancellationToken);
            return Results.Ok(access);
        }).RequireAuthorization();
    }
}
