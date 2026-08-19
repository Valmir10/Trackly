using FluentValidation.TestHelper;
using Trackly.Application.Features.Milestones.Commands.CreateMilestone;

namespace Trackly.Application.UnitTests.Features.Milestones.Commands.CreateMilestone;

public class CreateMilestoneCommandValidatorTests
{
    private readonly CreateMilestoneCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyContractId_HasError()
    {
        var result = _validator.TestValidate(new CreateMilestoneCommand(Guid.Empty, "Milestone 2"));
        result.ShouldHaveValidationErrorFor(x => x.ContractId);
    }

    [Fact]
    public void Validate_WithEmptyTitle_HasError()
    {
        var result = _validator.TestValidate(new CreateMilestoneCommand(Guid.NewGuid(), ""));
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new CreateMilestoneCommand(Guid.NewGuid(), "Milestone 2"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
