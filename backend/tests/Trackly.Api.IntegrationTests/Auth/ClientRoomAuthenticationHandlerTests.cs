using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using System.Net;
using System.Net.Http.Headers;
using Trackly.Api.Auth;
using Trackly.Application.Common.Interfaces;
using Trackly.Domain.Entities;

namespace Trackly.Api.IntegrationTests.Auth;

// Exercises ClientRoomAuthenticationHandler through the real ASP.NET Core
// authentication pipeline (via TestServer) rather than calling the
// protected HandleAuthenticateAsync directly — a minimal host with only the
// ClientRoom scheme wired up, no database, IClientRoomAccessRepository and
// IClientRoomTokenService substituted in.
public sealed class ClientRoomAuthenticationHandlerTests : IAsyncDisposable
{
    private readonly IClientRoomAccessRepository _accessRepository = Substitute.For<IClientRoomAccessRepository>();
    private readonly IClientRoomTokenService _tokenService = Substitute.For<IClientRoomTokenService>();
    private IHost? _host;

    private async Task<HttpClient> CreateClientAsync()
    {
        _host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddSingleton(_accessRepository);
                    services.AddSingleton(_tokenService);
                    services.AddAuthentication()
                        .AddScheme<AuthenticationSchemeOptions, ClientRoomAuthenticationHandler>(ClientRoomAuthDefaults.Scheme, _ => { });
                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy(ClientRoomAuthDefaults.Scheme, policy => policy
                            .AddAuthenticationSchemes(ClientRoomAuthDefaults.Scheme)
                            .RequireAuthenticatedUser());
                    });
                });
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseAuthentication();
                    app.UseAuthorization();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/probe", () => Results.Ok())
                            .RequireAuthorization(ClientRoomAuthDefaults.Scheme);
                    });
                });
            })
            .StartAsync();

        return _host.GetTestClient();
    }

    public async ValueTask DisposeAsync()
    {
        if (_host is not null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }
    }

    private static ClientRoomAccess ActiveAccess() =>
        ClientRoomAccess.Create(Guid.NewGuid(), Guid.NewGuid(), "hashed-token-value", DateTime.UtcNow.AddYears(1), Guid.NewGuid());

    [Fact]
    public async Task Probe_WithValidActiveToken_Succeeds()
    {
        // Arrange
        var access = ActiveAccess();
        _tokenService.Hash("raw-token").Returns("hashed-token-value");
        _accessRepository.GetByHashAsync("hashed-token-value", Arg.Any<CancellationToken>()).Returns(access);
        var client = await CreateClientAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "raw-token");

        // Act
        var response = await client.GetAsync("/probe");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Probe_WithRevokedToken_Fails()
    {
        // Arrange
        var access = ActiveAccess();
        access.Revoke();
        _tokenService.Hash("raw-token").Returns("hashed-token-value");
        _accessRepository.GetByHashAsync("hashed-token-value", Arg.Any<CancellationToken>()).Returns(access);
        var client = await CreateClientAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "raw-token");

        // Act
        var response = await client.GetAsync("/probe");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Probe_WithExpiredToken_Fails()
    {
        // Arrange — ClientRoomAccess.Create rejects a past ExpiresAt, so an
        // expired grant is built via a short-lived future expiry instead.
        var access = ClientRoomAccess.Create(Guid.NewGuid(), Guid.NewGuid(), "hashed-token-value", DateTime.UtcNow.AddMilliseconds(50), Guid.NewGuid());
        await Task.Delay(100);
        _tokenService.Hash("raw-token").Returns("hashed-token-value");
        _accessRepository.GetByHashAsync("hashed-token-value", Arg.Any<CancellationToken>()).Returns(access);
        var client = await CreateClientAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "raw-token");

        // Act
        var response = await client.GetAsync("/probe");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Probe_WithUnknownToken_Fails()
    {
        // Arrange
        _tokenService.Hash("raw-token").Returns("hashed-token-value");
        _accessRepository.GetByHashAsync("hashed-token-value", Arg.Any<CancellationToken>()).Returns((ClientRoomAccess?)null);
        var client = await CreateClientAsync();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "raw-token");

        // Act
        var response = await client.GetAsync("/probe");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Probe_WithNoAuthorizationHeader_Fails()
    {
        // Arrange
        var client = await CreateClientAsync();

        // Act
        var response = await client.GetAsync("/probe");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
