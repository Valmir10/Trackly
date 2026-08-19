using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using Trackly.Api.Auth;
using Trackly.Api.Endpoints;
using Trackly.Api.Hubs;
using Trackly.Api.Middleware;
using Trackly.Application;
using Trackly.Application.Common.Interfaces;
using Trackly.Infrastructure;
using Trackly.Infrastructure.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console());

builder.Services.AddOpenApi();

// Enums serialize as their name ("InReview"), not the underlying int — a
// bare 2 in a JSON response is an opaque, fragile contract for any client.
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// AddApplication() only scans Trackly.Application for MediatR handlers.
// The domain-event-to-SignalR notification handlers live here in Api, so
// they need their own registration — otherwise Publish() silently no-ops
// for them instead of erroring, since MediatR treats zero handlers for a
// notification as a valid (if useless) outcome.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICurrentClientRoomService, CurrentClientRoomService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// The default inbound claim mapping silently rewrites JWT-standard claim
// names (e.g. "sub") to legacy XML-schema URIs — disabling it keeps claim
// names exactly as JwtTokenService wrote them, which is what
// CurrentUserService/CurrentTenantService look for.
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt configuration section is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        };

        // SignalR sends the access token as an "access_token" query-string
        // param for WebSocket transport, not an Authorization header — this
        // is required now that the hub below carries .RequireAuthorization().
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments("/hubs/project"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    })
    .AddScheme<AuthenticationSchemeOptions, ClientRoomAuthenticationHandler>(ClientRoomAuthDefaults.Scheme, _ => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ClientRoomAuthDefaults.Scheme, policy => policy
        .AddAuthenticationSchemes(ClientRoomAuthDefaults.Scheme)
        .RequireAuthenticatedUser());
});

// AllowCredentials + a specific origin (not AllowAnyOrigin — browsers refuse
// that combination) so the HttpOnly refresh-token cookie and the SignalR
// connection both work across the frontend's separate origin.
const string FrontendCorsPolicy = "Frontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy => policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

var signalRBuilder = builder.Services.AddSignalR();
var redisConnectionString = builder.Configuration.GetSection("Redis")["ConnectionString"];
if (!string.IsNullOrEmpty(redisConnectionString))
{
    signalRBuilder.AddStackExchangeRedis(redisConnectionString);
}

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(FrontendCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapProjectEndpoints();
app.MapTicketEndpoints();
app.MapChatEndpoints();
app.MapMeetingEndpoints();
app.MapDecisionEndpoints();
app.MapContractEndpoints();
app.MapMilestoneEndpoints();
app.MapClientRoomAccessEndpoints();
app.MapClientRoomEndpoints();

app.MapHub<ProjectHub>("/hubs/project").RequireAuthorization();

app.Run();
