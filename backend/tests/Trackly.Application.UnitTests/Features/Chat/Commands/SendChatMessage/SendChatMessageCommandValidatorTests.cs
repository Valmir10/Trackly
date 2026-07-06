using FluentValidation.TestHelper;
using Trackly.Application.Features.Chat.Commands.SendChatMessage;

namespace Trackly.Application.UnitTests.Features.Chat.Commands.SendChatMessage;

public class SendChatMessageCommandValidatorTests
{
    private readonly SendChatMessageCommandValidator _validator = new();

    // -------------------------------------------------------
    // ProjectId
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new SendChatMessageCommand(Guid.Empty, null, "hello"));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    // -------------------------------------------------------
    // Content
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithEmptyContent_HasError()
    {
        var result = _validator.TestValidate(new SendChatMessageCommand(Guid.NewGuid(), null, ""));
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Validate_WithContentOver4000Characters_HasError()
    {
        var longContent = new string('a', 4001);
        var result = _validator.TestValidate(new SendChatMessageCommand(Guid.NewGuid(), null, longContent));
        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    // -------------------------------------------------------
    // Valid command
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new SendChatMessageCommand(Guid.NewGuid(), Guid.NewGuid(), "hello"));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
