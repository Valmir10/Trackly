using MediatR;
using Trackly.Application.Features.Auth.Common;

namespace Trackly.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(
    string TenantName,
    string TenantSlug,
    string Email,
    string Password,
    string FirstName,
    string LastName) : IRequest<AuthResult>;
