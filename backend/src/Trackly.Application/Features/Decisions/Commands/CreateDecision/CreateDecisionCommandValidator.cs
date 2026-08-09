using FluentValidation;

namespace Trackly.Application.Features.Decisions.Commands.CreateDecision;

public sealed class CreateDecisionCommandValidator : AbstractValidator<CreateDecisionCommand>
{
    public CreateDecisionCommandValidator()
    {
        RuleFor(x => x.MeetingId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty().MaximumLength(1000);
    }
}
