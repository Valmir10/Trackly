using FluentValidation;

namespace Trackly.Application.Features.ClientRoom.Commands.RevokeAccess;

public sealed class RevokeClientRoomAccessCommandValidator : AbstractValidator<RevokeClientRoomAccessCommand>
{
    public RevokeClientRoomAccessCommandValidator()
    {
        RuleFor(x => x.AccessId).NotEmpty();
    }
}
