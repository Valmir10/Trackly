using FluentValidation.TestHelper;
using Trackly.Application.Features.Auth.Commands.RefreshAccessToken;

namespace Trackly.Application.UnitTests.Features.Auth.Commands.RefreshAccessToken;

public class RefreshAccessTokenCommandValidatorTests
{
    private readonly RefreshAccessTokenCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyRefreshToken_HasError()
    {
        var result = _validator.TestValidate(new RefreshAccessTokenCommand(""));
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken);
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new RefreshAccessTokenCommand("some-raw-token"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
