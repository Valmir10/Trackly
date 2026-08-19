using FluentValidation.TestHelper;
using Trackly.Application.Features.Milestones.Commands.ApproveMilestone;

namespace Trackly.Application.UnitTests.Features.Milestones.Commands.ApproveMilestone;

public class ApproveMilestoneCommandValidatorTests
{
    private readonly ApproveMilestoneCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyMilestoneId_HasError()
    {
        var result = _validator.TestValidate(new ApproveMilestoneCommand(Guid.Empty));
        result.ShouldHaveValidationErrorFor(x => x.MilestoneId);
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new ApproveMilestoneCommand(Guid.NewGuid()));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
