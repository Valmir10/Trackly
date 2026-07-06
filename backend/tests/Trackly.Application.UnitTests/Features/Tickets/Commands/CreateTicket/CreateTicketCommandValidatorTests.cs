using FluentValidation.TestHelper;
using Trackly.Application.Features.Tickets.Commands.CreateTicket;
using Trackly.Domain.Enums;

namespace Trackly.Application.UnitTests.Features.Tickets.Commands.CreateTicket;

public class CreateTicketCommandValidatorTests
{
    private readonly CreateTicketCommandValidator _validator = new();

    // -------------------------------------------------------
    // ProjectId
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new CreateTicketCommand(Guid.Empty, "Set up Storybook", null, TicketPriority.Medium, null, null));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    // -------------------------------------------------------
    // Title
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithEmptyTitle_HasError()
    {
        var result = _validator.TestValidate(new CreateTicketCommand(Guid.NewGuid(), "", null, TicketPriority.Medium, null, null));
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    // -------------------------------------------------------
    // Valid command
    // -------------------------------------------------------

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        var result = _validator.TestValidate(new CreateTicketCommand(Guid.NewGuid(), "Set up Storybook", null, TicketPriority.Medium, null, null));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
