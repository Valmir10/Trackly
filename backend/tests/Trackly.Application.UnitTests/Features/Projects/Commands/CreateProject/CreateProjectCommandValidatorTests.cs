using FluentValidation.TestHelper;
using Trackly.Application.Features.Projects.Commands.CreateProject;

namespace Trackly.Application.UnitTests.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator = new();

    // -------------------------------------------------------
    // Name
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithEmptyName_HasError()
    {
        var result = _validator.TestValidate(new CreateProjectCommand("", "var(--tp-cat-1)", null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameOver100Characters_HasError()
    {
        var longName = new string('a', 101);
        var result = _validator.TestValidate(new CreateProjectCommand(longName, "var(--tp-cat-1)", null));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    // -------------------------------------------------------
    // Color
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithEmptyColor_HasError()
    {
        var result = _validator.TestValidate(new CreateProjectCommand("Frontend redesign", "", null));
        result.ShouldHaveValidationErrorFor(x => x.Color);
    }

    // -------------------------------------------------------
    // Valid command
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new CreateProjectCommand("Frontend redesign", "var(--tp-cat-1)", "A description"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
