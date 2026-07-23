using MediatR;
using Trackly.Application.Features.Auth.Common;

namespace Trackly.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string TenantSlug, string Email, string Password) : IRequest<AuthResult>;
