using FluentValidation;

namespace Trackly.Application.Features.Tickets.Commands.ChangeTicketStatus;

public sealed class ChangeTicketStatusCommandValidator : AbstractValidator<ChangeTicketStatusCommand>
{
    public ChangeTicketStatusCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.NewStatus).IsInEnum();
        RuleFor(x => x.Position).GreaterThanOrEqualTo(0);
    }
}
