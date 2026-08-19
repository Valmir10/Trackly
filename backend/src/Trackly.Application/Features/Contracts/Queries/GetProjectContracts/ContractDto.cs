namespace Trackly.Application.Features.Contracts.Queries.GetProjectContracts;

public sealed record ContractDto(Guid Id, Guid ProjectId, string Title, DateTime CreatedAt);
