using MediatR;
using Trackly.Application.Features.Decisions.Commands.CreateDecision;
using Trackly.Application.Features.Decisions.Queries.GetMeetingDecisions;
using Trackly.Application.Features.Decisions.Queries.GetWorkspaceDecisions;

namespace Trackly.Api.Endpoints;

public sealed record CreateDecisionRequest(string Text);

public static class DecisionEndpoints
{
    public static void MapDecisionEndpoints(this WebApplication app)
    {
        app.MapPost("/api/meetings/{meetingId:guid}/decisions", async (
            Guid meetingId, CreateDecisionRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateDecisionCommand(meetingId, request.Text);
            var decisionId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/meetings/{meetingId}/decisions/{decisionId}", new { Id = decisionId });
        }).RequireAuthorization();

        app.MapGet("/api/meetings/{meetingId:guid}/decisions", async (
            Guid meetingId, ISender sender, CancellationToken cancellationToken) =>
        {
            var decisions = await sender.Send(new GetMeetingDecisionsQuery(meetingId), cancellationToken);
            return Results.Ok(decisions);
        }).RequireAuthorization();

        app.MapGet("/api/decisions", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var decisions = await sender.Send(new GetWorkspaceDecisionsQuery(), cancellationToken);
            return Results.Ok(decisions);
        }).RequireAuthorization();
    }
}
