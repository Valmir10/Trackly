using FluentValidation.TestHelper;
using Trackly.Application.Features.Tickets.Commands.SetTicketBlockedByTicket;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.SetTicketBlockedByTicket;

public class SetTicketBlockedByTicketCommandValidatorTests
{
    private readonly SetTicketBlockedByTicketCommandValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyTicketId_HasError()
    {
        var result = _validator.TestValidate(new SetTicketBlockedByTicketCommand(Guid.Empty, Guid.NewGuid()));
        result.ShouldHaveValidationErrorFor(x => x.TicketId);
    }

    [Fact]
    public void Validate_WithTicketBlockingItself_HasError()
    {
        var ticketId = Guid.NewGuid();
        var result = _validator.TestValidate(new SetTicketBlockedByTicketCommand(ticketId, ticketId));
        result.ShouldHaveValidationErrorFor("BlockingTicketId");
    }

    [Fact]
    public void Validate_WithNullBlockingTicketId_HasNoErrors()
    {
        var result = _validator.TestValidate(new SetTicketBlockedByTicketCommand(Guid.NewGuid(), null));
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new SetTicketBlockedByTicketCommand(Guid.NewGuid(), Guid.NewGuid()));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
