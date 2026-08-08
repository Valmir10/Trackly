using FluentValidation;

namespace Trackly.Application.Features.Chat.Queries.GetChatMessages;

public sealed class GetChatMessagesQueryValidator : AbstractValidator<GetChatMessagesQuery>
{
    public GetChatMessagesQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
