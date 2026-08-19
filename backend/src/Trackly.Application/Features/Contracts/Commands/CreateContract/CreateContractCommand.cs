using MediatR;

namespace Trackly.Application.Features.Contracts.Commands.CreateContract;

public sealed record CreateContractCommand(Guid ProjectId, string Title) : IRequest<Guid>;
