using FluentValidation.TestHelper;
using Trackly.Application.Features.Meetings.Commands.CreateMeeting;

namespace Trackly.Application.UnitTests.Features.Meetings.Commands.CreateMeeting;

public class CreateMeetingCommandValidatorTests
{
    private readonly CreateMeetingCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new CreateMeetingCommand(Guid.Empty, "Sprint Planning 14", DateTime.UtcNow));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void Validate_WithEmptyTitle_HasError()
    {
        var result = _validator.TestValidate(new CreateMeetingCommand(Guid.NewGuid(), "", DateTime.UtcNow));
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new CreateMeetingCommand(Guid.NewGuid(), "Sprint Planning 14", DateTime.UtcNow));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
