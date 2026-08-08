using MediatR;

namespace Trackly.Application.Features.Projects.Queries.GetProjects;

public sealed record GetProjectsQuery : IRequest<IReadOnlyList<ProjectDto>>;
