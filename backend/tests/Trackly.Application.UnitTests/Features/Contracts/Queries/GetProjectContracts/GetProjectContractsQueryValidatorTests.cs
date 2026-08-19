using FluentValidation.TestHelper;
using Trackly.Application.Features.Contracts.Queries.GetProjectContracts;

namespace Trackly.Application.UnitTests.Features.Contracts.Queries.GetProjectContracts;

public class GetProjectContractsQueryValidatorTests
{
    private readonly GetProjectContractsQueryValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new GetProjectContractsQuery(Guid.Empty));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void Validate_WithValidQuery_HasNoErrors()
    {
        var result = _validator.TestValidate(new GetProjectContractsQuery(Guid.NewGuid()));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
