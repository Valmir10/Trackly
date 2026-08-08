using FluentValidation.TestHelper;
using Trackly.Application.Features.Chat.Queries.GetChatMessages;

namespace Trackly.Application.UnitTests.Features.Chat.Queries.GetChatMessages;

public class GetChatMessagesQueryValidatorTests
{
    private readonly GetChatMessagesQueryValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new GetChatMessagesQuery(Guid.Empty, null));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void Validate_WithValidProjectId_HasNoErrors()
    {
        var result = _validator.TestValidate(new GetChatMessagesQuery(Guid.NewGuid(), null));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
