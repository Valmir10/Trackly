using FluentValidation;

namespace Trackly.Application.Features.Meetings.Commands.UpdateMeetingNotes;

public sealed class UpdateMeetingNotesCommandValidator : AbstractValidator<UpdateMeetingNotesCommand>
{
    public UpdateMeetingNotesCommandValidator()
    {
        RuleFor(x => x.MeetingId).NotEmpty();
        RuleFor(x => x.Notes).MaximumLength(20000);
    }
}
