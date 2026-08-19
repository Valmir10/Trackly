using FluentValidation;

namespace Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByMilestone;

public sealed class SetTicketBlockedByMilestoneCommandValidator : AbstractValidator<SetTicketBlockedByMilestoneCommand>
{
    public SetTicketBlockedByMilestoneCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
    }
}
