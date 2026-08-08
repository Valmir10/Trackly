using MediatR;
using Trackly.Application.Features.Chat.Commands.SendChatMessage;
using Trackly.Application.Features.Chat.Queries.GetChatMessages;

namespace Trackly.Api.Endpoints;

public static class ChatEndpoints
{
    public static void MapChatEndpoints(this WebApplication app)
    {
        app.MapPost("/api/chat/messages", async (SendChatMessageCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var messageId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/chat/messages/{messageId}", new { Id = messageId });
        }).RequireAuthorization();

        app.MapGet("/api/chat/messages", async (
            Guid projectId, Guid? ticketId, ISender sender, CancellationToken cancellationToken) =>
        {
            var messages = await sender.Send(new GetChatMessagesQuery(projectId, ticketId), cancellationToken);
            return Results.Ok(messages);
        }).RequireAuthorization();
    }
}
