using FluentValidation;

namespace Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByTicket;

public sealed class SetTicketBlockedByTicketCommandValidator : AbstractValidator<SetTicketBlockedByTicketCommand>
{
    public SetTicketBlockedByTicketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x)
            .Must(x => x.BlockingTicketId != x.TicketId)
            .WithMessage("A ticket cannot block itself.")
            .WithName("BlockingTicketId");
    }
}
