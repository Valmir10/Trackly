using FluentValidation.TestHelper;
using Trackly.Application.Features.Contracts.Commands.CreateContract;

namespace Trackly.Application.UnitTests.Features.Contracts.Commands.CreateContract;

public class CreateContractCommandValidatorTests
{
    private readonly CreateContractCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new CreateContractCommand(Guid.Empty, "Meridian SOW"));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void Validate_WithEmptyTitle_HasError()
    {
        var result = _validator.TestValidate(new CreateContractCommand(Guid.NewGuid(), ""));
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new CreateContractCommand(Guid.NewGuid(), "Meridian SOW"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
