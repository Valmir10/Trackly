using FluentValidation;

namespace Trackly.Application.Features.Contracts.Queries.GetProjectContracts;

public sealed class GetProjectContractsQueryValidator : AbstractValidator<GetProjectContractsQuery>
{
    public GetProjectContractsQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
