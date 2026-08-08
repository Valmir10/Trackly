using FluentValidation;

namespace Trackly.Application.Features.Tickets.Queries.GetProjectTickets;

public sealed class GetProjectTicketsQueryValidator : AbstractValidator<GetProjectTicketsQuery>
{
    public GetProjectTicketsQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
