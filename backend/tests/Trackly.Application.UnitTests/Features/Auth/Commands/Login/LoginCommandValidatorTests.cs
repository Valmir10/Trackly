using FluentValidation.TestHelper;
using Trackly.Application.Features.Auth.Commands.Login;

namespace Trackly.Application.UnitTests.Features.Auth.Commands.Login;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyTenantSlug_HasError()
    {
        var result = _validator.TestValidate(new LoginCommand("", "john@acme.com", "password"));
        result.ShouldHaveValidationErrorFor(x => x.TenantSlug);
    }

    [Fact]
    public void Validate_WithInvalidEmail_HasError()
    {
        var result = _validator.TestValidate(new LoginCommand("acme-corp", "not-an-email", "password"));
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Validate_WithEmptyPassword_HasError()
    {
        var result = _validator.TestValidate(new LoginCommand("acme-corp", "john@acme.com", ""));
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new LoginCommand("acme-corp", "john@acme.com", "password"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
