using MediatR;

namespace Trackly.Application.Features.Projects.Commands.CreateProject;

public sealed record CreateProjectCommand(string Name, string Color, string? Description) : IRequest<Guid>;
