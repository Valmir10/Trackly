using FluentValidation.TestHelper;
using Trackly.Application.Features.Meetings.Commands.UpdateMeetingNotes;

namespace Trackly.Application.UnitTests.Features.Meetings.Commands.UpdateMeetingNotes;

public class UpdateMeetingNotesCommandValidatorTests
{
    private readonly UpdateMeetingNotesCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyMeetingId_HasError()
    {
        var result = _validator.TestValidate(new UpdateMeetingNotesCommand(Guid.Empty, "Some notes"));
        result.ShouldHaveValidationErrorFor(x => x.MeetingId);
    }

    [Fact]
    public void Validate_WithNotesOverMaxLength_HasError()
    {
        var result = _validator.TestValidate(new UpdateMeetingNotesCommand(Guid.NewGuid(), new string('a', 20001)));
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Validate_WithEmptyNotes_HasNoErrors()
    {
        var result = _validator.TestValidate(new UpdateMeetingNotesCommand(Guid.NewGuid(), ""));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new UpdateMeetingNotesCommand(Guid.NewGuid(), "Discussed the roadmap."));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
