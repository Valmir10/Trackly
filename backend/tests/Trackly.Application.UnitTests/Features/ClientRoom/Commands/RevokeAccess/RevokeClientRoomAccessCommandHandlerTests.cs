using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.ClientRoom.Commands.RevokeAccess;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.ClientRoom.Commands.RevokeAccess;

public class RevokeClientRoomAccessCommandHandlerTests
{
    private readonly IClientRoomAccessRepository _accessRepository = Substitute.For<IClientRoomAccessRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private RevokeClientRoomAccessCommandHandler CreateHandler() => new(_accessRepository, _unitOfWork);

    private static ClientRoomAccess ExistingAccess() =>
        ClientRoomAccess.Create(Guid.NewGuid(), Guid.NewGuid(), "hashed-token-value", DateTime.UtcNow.AddYears(1), Guid.NewGuid());

    [Fact]
    public async Task Handle_RevokesAccess()
    {
        // Arrange
        var access = ExistingAccess();
        _accessRepository.GetByIdAsync(access.Id, Arg.Any<CancellationToken>()).Returns(access);
        var handler = CreateHandler();

        // Act
        await handler.Handle(new RevokeClientRoomAccessCommand(access.Id), CancellationToken.None);

        // Assert
        access.IsRevoked.Should().BeTrue();
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenAccessDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingAccessId = Guid.NewGuid();
        _accessRepository.GetByIdAsync(missingAccessId, Arg.Any<CancellationToken>()).Returns((ClientRoomAccess?)null);
        var handler = CreateHandler();

        // Act
        var act = () => handler.Handle(new RevokeClientRoomAccessCommand(missingAccessId), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
