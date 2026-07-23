using MediatR;
using Trackly.Application.Features.Auth.Common;

namespace Trackly.Application.Features.Auth.Commands.RefreshAccessToken;

public sealed record RefreshAccessTokenCommand(string RefreshToken) : IRequest<AuthResult>;
