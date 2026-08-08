namespace Trackly.Application.Features.Projects.Queries.GetProjects;

public sealed record ProjectDto(Guid Id, string Name, string Color, string? Description);
