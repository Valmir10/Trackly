using FluentValidation.TestHelper;
using Trackly.Application.Features.Decisions.Commands.CreateDecision;

namespace Trackly.Application.UnitTests.Features.Decisions.Commands.CreateDecision;

public class CreateDecisionCommandValidatorTests
{
    private readonly CreateDecisionCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyMeetingId_HasError()
    {
        var result = _validator.TestValidate(new CreateDecisionCommand(Guid.Empty, "Ship v2 by Friday"));
        result.ShouldHaveValidationErrorFor(x => x.MeetingId);
    }

    [Fact]
    public void Validate_WithEmptyText_HasError()
    {
        var result = _validator.TestValidate(new CreateDecisionCommand(Guid.NewGuid(), ""));
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }

    [Fact]
    public void Validate_WithTextOverMaxLength_HasError()
    {
        var result = _validator.TestValidate(new CreateDecisionCommand(Guid.NewGuid(), new string('a', 1001)));
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new CreateDecisionCommand(Guid.NewGuid(), "Ship v2 by Friday"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
