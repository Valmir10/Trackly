using MediatR;

namespace Trackly.Application.Features.Contracts.Queries.GetProjectContracts;

public sealed record GetProjectContractsQuery(Guid ProjectId) : IRequest<IReadOnlyList<ContractDto>>;
