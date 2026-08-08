using FluentValidation.TestHelper;
using Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

namespace Trackly.Application.UnitTests.Features.Tickets.Queries.GetProjectTickets;

public class GetProjectTicketsQueryValidatorTests
{
    private readonly GetProjectTicketsQueryValidator _validator = new();

    [Fact]
    public void Validate_WithEmptyProjectId_HasError()
    {
        var result = _validator.TestValidate(new GetProjectTicketsQuery(Guid.Empty));
        result.ShouldHaveValidationErrorFor(x => x.ProjectId);
    }

    [Fact]
    public void Validate_WithValidProjectId_HasNoErrors()
    {
        var result = _validator.TestValidate(new GetProjectTicketsQuery(Guid.NewGuid()));
        result.ShouldNotHaveAnyValidationErrors();
    }
}
