using MediatR;
using Trackly.Application.Features.Projects.Commands.CreateProject;

namespace Trackly.Api.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/projects").RequireAuthorization();

        group.MapPost("/", async (CreateProjectCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            var projectId = await sender.Send(command, cancellationToken);
            return Results.Created($"/api/projects/{projectId}", new { Id = projectId });
        });
    }
}
