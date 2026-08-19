using FluentValidation;

namespace Trackly.Application.Features.Milestones.Commands.ApproveMilestone;

public sealed class ApproveMilestoneCommandValidator : AbstractValidator<ApproveMilestoneCommand>
{
    public ApproveMilestoneCommandValidator()
    {
        RuleFor(x => x.MilestoneId).NotEmpty();
    }
}
