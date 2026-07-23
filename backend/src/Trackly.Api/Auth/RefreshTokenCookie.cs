namespace Trackly.Api.Auth;

public static class RefreshTokenCookie
{
    private const string CookieName = "refreshToken";

    public static void Set(HttpContext httpContext, string rawToken)
    {
        httpContext.Response.Cookies.Append(CookieName, rawToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7),
        });
    }

    public static string? Read(HttpContext httpContext)
    {
        return httpContext.Request.Cookies[CookieName];
    }
}
