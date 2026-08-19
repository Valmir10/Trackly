using FluentValidation;

namespace Trackly.Application.Features.Milestones.Queries.GetProjectMilestones;

public sealed class GetProjectMilestonesQueryValidator : AbstractValidator<GetProjectMilestonesQuery>
{
    public GetProjectMilestonesQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
