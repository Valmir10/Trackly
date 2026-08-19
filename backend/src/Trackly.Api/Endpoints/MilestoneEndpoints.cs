using MediatR;
using Trackly.Application.Features.Milestones.Commands.CreateMilestone;
using Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;
using Trackly.Application.Features.Milestones.Queries.GetWorkspaceMilestones;

namespace Trackly.Api.Endpoints;

public sealed record CreateMilestoneRequest(string Title);

public static class MilestoneEndpoints
{
    public static void MapMilestoneEndpoints(this WebApplication app)
    {
        app.MapPost("/api/contracts/{contractId:guid}/milestones", async (
            Guid contractId, CreateMilestoneRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateMilestoneCommand(contractId, request.Title);
            var milestoneId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/milestones/{milestoneId}", new { Id = milestoneId });
        }).RequireAuthorization();

        app.MapGet("/api/projects/{projectId:guid}/milestones", async (
            Guid projectId, ISender sender, CancellationToken cancellationToken) =>
        {
            var milestones = await sender.Send(new GetProjectMilestonesQuery(projectId), cancellationToken);
            return Results.Ok(milestones);
        }).RequireAuthorization();

        app.MapGet("/api/milestones", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var milestones = await sender.Send(new GetWorkspaceMilestonesQuery(), cancellationToken);
            return Results.Ok(milestones);
        }).RequireAuthorization();
    }
}
