using MediatR;
using Trackly.Application.Common.Interfaces;

namespace Trackly.Application.Features.Contracts.Queries.GetProjectContracts;

public sealed class GetProjectContractsQueryHandler : IRequestHandler<GetProjectContractsQuery, IReadOnlyList<ContractDto>>
{
    private readonly IContractRepository _contractRepository;

    public GetProjectContractsQueryHandler(IContractRepository contractRepository)
    {
        _contractRepository = contractRepository;
    }

    public async Task<IReadOnlyList<ContractDto>> Handle(GetProjectContractsQuery request, CancellationToken cancellationToken)
    {
        var contracts = await _contractRepository.GetByProjectIdAsync(request.ProjectId, cancellationToken);
        return contracts
            .Select(c => new ContractDto(c.Id, c.ProjectId, c.Title, c.CreatedAt))
            .ToList();
    }
}
