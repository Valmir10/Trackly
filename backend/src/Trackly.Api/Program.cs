using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MediatR;
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
    });

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapProjectEndpoints();
app.MapTicketEndpoints();
app.MapChatEndpoints();

app.MapHub<ProjectHub>("/hubs/project");

app.Run();
