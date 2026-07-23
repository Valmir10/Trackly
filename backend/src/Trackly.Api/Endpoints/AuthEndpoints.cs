using MediatR;
using Trackly.Api.Auth;
using Trackly.Application.Features.Auth.Commands.Login;
using Trackly.Application.Features.Auth.Commands.RefreshAccessToken;
using Trackly.Application.Features.Auth.Commands.Register;
using Trackly.Application.Features.Auth.Common;

namespace Trackly.Api.Endpoints;

public sealed record AuthResponse(Guid UserId, Guid TenantId, string AccessToken);

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", async (RegisterCommand command, ISender sender, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(ToResponse(result, httpContext));
        });

        group.MapPost("/login", async (LoginCommand command, ISender sender, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(command, cancellationToken);
            return Results.Ok(ToResponse(result, httpContext));
        });

        group.MapPost("/refresh", async (ISender sender, HttpContext httpContext, CancellationToken cancellationToken) =>
        {
            var rawToken = RefreshTokenCookie.Read(httpContext);
            if (string.IsNullOrEmpty(rawToken))
            {
                return Results.Unauthorized();
            }

            var result = await sender.Send(new RefreshAccessTokenCommand(rawToken), cancellationToken);
            return Results.Ok(ToResponse(result, httpContext));
        });
    }

    private static AuthResponse ToResponse(AuthResult result, HttpContext httpContext)
    {
        RefreshTokenCookie.Set(httpContext, result.RefreshToken);
        return new AuthResponse(result.UserId, result.TenantId, result.AccessToken);
    }
}
