using MediatR;
using Trackly.Application.Features.Meetings.Commands.CreateMeeting;
using Trackly.Application.Features.Meetings.Commands.UpdateMeetingNotes;
using Trackly.Application.Features.Meetings.Queries.GetMeetingById;
using Trackly.Application.Features.Meetings.Queries.GetProjectMeetings;
using Trackly.Application.Features.Meetings.Queries.GetWorkspaceMeetings;

namespace Trackly.Api.Endpoints;

public sealed record CreateMeetingRequest(string Title, DateTime ScheduledAt);

public sealed record UpdateMeetingNotesRequest(string Notes);

public static class MeetingEndpoints
{
    public static void MapMeetingEndpoints(this WebApplication app)
    {
        app.MapPost("/api/projects/{projectId:guid}/meetings", async (
            Guid projectId, CreateMeetingRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateMeetingCommand(projectId, request.Title, request.ScheduledAt);
            var meetingId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/meetings/{meetingId}", new { Id = meetingId });
        }).RequireAuthorization();

        app.MapGet("/api/projects/{projectId:guid}/meetings", async (
            Guid projectId, ISender sender, CancellationToken cancellationToken) =>
        {
            var meetings = await sender.Send(new GetProjectMeetingsQuery(projectId), cancellationToken);
            return Results.Ok(meetings);
        }).RequireAuthorization();

        app.MapGet("/api/meetings", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var meetings = await sender.Send(new GetWorkspaceMeetingsQuery(), cancellationToken);
            return Results.Ok(meetings);
        }).RequireAuthorization();

        app.MapGet("/api/meetings/{meetingId:guid}", async (
            Guid meetingId, ISender sender, CancellationToken cancellationToken) =>
        {
            var meeting = await sender.Send(new GetMeetingByIdQuery(meetingId), cancellationToken);
            return Results.Ok(meeting);
        }).RequireAuthorization();

        app.MapPatch("/api/meetings/{meetingId:guid}/notes", async (
            Guid meetingId, UpdateMeetingNotesRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new UpdateMeetingNotesCommand(meetingId, request.Notes), cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization();
    }
}
