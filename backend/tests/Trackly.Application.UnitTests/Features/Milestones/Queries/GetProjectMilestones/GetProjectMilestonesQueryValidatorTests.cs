using FluentValidation.TestHelper;
using Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;

namespace Trackly.Application.UnitTests.Features.Milestones.Queries.GetProjectMilestones;

public class GetProjectMilestonesQueryValidatorTests
{
    private readonly GetProjectMilestonesQueryValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new GetProjectMilestonesQuery(Guid.Empty));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void Validate_WithValidQuery_HasNoErrors()
    {
        var result = _validator.TestValidate(new GetProjectMilestonesQuery(Guid.NewGuid()));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
