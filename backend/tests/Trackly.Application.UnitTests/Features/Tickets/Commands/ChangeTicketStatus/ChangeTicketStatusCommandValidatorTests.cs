using FluentValidation.TestHelper;
using Trackly.Application.Features.Tickets.Commands.ChangeTicketStatus;
using Trackly.Domain.Enums;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.ChangeTicketStatus;

public class ChangeTicketStatusCommandValidatorTests
{
    private readonly ChangeTicketStatusCommandValidator _validator = new();

    // -------------------------------------------------------
    // TicketId
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithEmptyTicketId_HasError()
    {
        var result = _validator.TestValidate(new ChangeTicketStatusCommand(Guid.Empty, TicketStatus.Done, 0));
        result.ShouldHaveValidationErrorFor(x => x.TicketId);
    }

    // -------------------------------------------------------
    // Position
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithNegativePosition_HasError()
    {
        var result = _validator.TestValidate(new ChangeTicketStatusCommand(Guid.NewGuid(), TicketStatus.Done, -1));
        result.ShouldHaveValidationErrorFor(x => x.Position);
    }

    // -------------------------------------------------------
    // Valid command
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new ChangeTicketStatusCommand(Guid.NewGuid(), TicketStatus.Done, 0));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
