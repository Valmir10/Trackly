using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.ClientRoom.Queries.GetProjectAccess;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.ClientRoom.Queries.GetProjectAccess;

public class GetProjectClientRoomAccessQueryHandlerTests
{
    private readonly IClientRoomAccessRepository _accessRepository = Substitute.For<IClientRoomAccessRepository>();

    private GetProjectClientRoomAccessQueryHandler CreateHandler() => new(_accessRepository);

    [Fact]
    public async Task Handle_ReturnsAccessGrantsMappedToDtos_WithoutTokenHash()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var access = ClientRoomAccess.Create(Guid.NewGuid(), projectId, "hashed-token-value", DateTime.UtcNow.AddYears(1), Guid.NewGuid());
        _accessRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<ClientRoomAccess> { access });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectClientRoomAccessQuery(projectId), CancellationToken.None);

        // Assert
        result.Should().ContainSingle();
        result[0].Id.Should().Be(access.Id);
        result[0].IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithNoAccessGrants_ReturnsEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _accessRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<ClientRoomAccess>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectClientRoomAccessQuery(projectId), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
