using FluentValidation.TestHelper;
using Trackly.Application.Features.Auth.Commands.Register;

namespace Trackly.Application.UnitTests.Features.Auth.Commands.Register;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator = new();

    private static RegisterCommand ValidCommand() => new(
        "Acme Corp", "acme-corp", "john@acme.com", "supersecurepassword", "John", "Smith");

    // -------------------------------------------------------
    // TenantSlug
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithUppercaseSlug_HasError()
    {
        var command = ValidCommand() with { TenantSlug = "Acme-Corp" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TenantSlug);
    }

    [Fact]
    public void Validate_WithSpacesInSlug_HasError()
    {
        var command = ValidCommand() with { TenantSlug = "acme corp" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TenantSlug);
    }

    // -------------------------------------------------------
    // Email
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithInvalidEmail_HasError()
    {
        var command = ValidCommand() with { Email = "not-an-email" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    // -------------------------------------------------------
    // Password
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithPasswordUnder10Characters_HasError()
    {
        var command = ValidCommand() with { Password = "short" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    // -------------------------------------------------------
    // Valid command
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(ValidCommand());
        result.ShouldNotHaveAnyValidationErrors();
    }
}
