using FluentValidation;

namespace Trackly.Application.Features.ClientRoom.Commands.CreateAccess;

public sealed class CreateClientRoomAccessCommandValidator : AbstractValidator<CreateClientRoomAccessCommand>
{
    public CreateClientRoomAccessCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
