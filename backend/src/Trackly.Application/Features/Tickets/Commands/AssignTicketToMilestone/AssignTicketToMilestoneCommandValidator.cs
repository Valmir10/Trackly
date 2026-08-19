using FluentValidation;

namespace Trackly.Application.Features.Tickets.Commands.AssignTicketToMilestone;

public sealed class AssignTicketToMilestoneCommandValidator : AbstractValidator<AssignTicketToMilestoneCommand>
{
    public AssignTicketToMilestoneCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
    }
}
