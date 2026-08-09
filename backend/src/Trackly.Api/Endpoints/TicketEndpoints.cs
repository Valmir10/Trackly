using MediatR;
using Trackly.Application.Features.Tickets.Commands.ChangeTicketStatus;
using Trackly.Application.Features.Tickets.Commands.CreateTicket;
using Trackly.Application.Features.Tickets.Queries.GetProjectTickets;
using Trackly.Application.Features.Tickets.Queries.GetWorkspaceTickets;
using Trackly.Domain.Enums;

namespace Trackly.Api.Endpoints;

public sealed record CreateTicketRequest(string Title, string? Description, TicketPriority Priority, Guid? AssignedToId, DateTime? DueDate);

public sealed record ChangeTicketStatusRequest(TicketStatus NewStatus, int Position);

public static class TicketEndpoints
{
    public static void MapTicketEndpoints(this WebApplication app)
    {
        app.MapPost("/api/projects/{projectId:guid}/tickets", async (
            Guid projectId, CreateTicketRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new CreateTicketCommand(
                projectId, request.Title, request.Description, request.Priority, request.AssignedToId, request.DueDate);
            var ticketId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/tickets/{ticketId}", new { Id = ticketId });
        }).RequireAuthorization();

        app.MapPatch("/api/tickets/{ticketId:guid}/status", async (
            Guid ticketId, ChangeTicketStatusRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new ChangeTicketStatusCommand(ticketId, request.NewStatus, request.Position), cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization();

        app.MapGet("/api/projects/{projectId:guid}/tickets", async (
            Guid projectId, ISender sender, CancellationToken cancellationToken) =>
        {
            var tickets = await sender.Send(new GetProjectTicketsQuery(projectId), cancellationToken);
            return Results.Ok(tickets);
        }).RequireAuthorization();

        app.MapGet("/api/tickets", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var tickets = await sender.Send(new GetWorkspaceTicketsQuery(), cancellationToken);
            return Results.Ok(tickets);
        }).RequireAuthorization();
    }
}
