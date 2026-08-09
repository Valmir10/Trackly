using FluentValidation;

namespace Trackly.Application.Features.Meetings.Commands.CreateMeeting;

public sealed class CreateMeetingCommandValidator : AbstractValidator<CreateMeetingCommand>
{
    public CreateMeetingCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ScheduledAt).NotEmpty();
    }
}
