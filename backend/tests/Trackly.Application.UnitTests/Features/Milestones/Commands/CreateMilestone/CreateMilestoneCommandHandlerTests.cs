using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Exceptions;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Milestones.Commands.CreateMilestone;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Milestones.Commands.CreateMilestone;

public class CreateMilestoneCommandHandlerTests
{
    private readonly IMilestoneRepository _milestoneRepository = Substitute.For<IMilestoneRepository>();
    private readonly IContractRepository _contractRepository = Substitute.For<IContractRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ICurrentUserService _currentUser = Substitute.For<ICurrentUserService>();

    private CreateMilestoneCommandHandler CreateHandler() =>
        new(_milestoneRepository, _contractRepository, _unitOfWork, _currentUser);

    private static Contract ExistingContract() =>
        Contract.Create(Guid.NewGuid(), Guid.NewGuid(), "Meridian SOW", Guid.NewGuid());

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsNewMilestoneId()
    {
        // Arrange
        var contract = ExistingContract();
        _contractRepository.GetByIdAsync(contract.Id, Arg.Any<CancellationToken>()).Returns(contract);
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateMilestoneCommand(contract.Id, "Milestone 2");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_DerivesProjectAndTenantFromTheLoadedContract()
    {
        // Arrange — the command itself carries no ProjectId/TenantId, which
        // is the point: mirrors CreateDecisionCommandHandler's
        // derive-from-parent defense.
        var contract = ExistingContract();
        _contractRepository.GetByIdAsync(contract.Id, Arg.Any<CancellationToken>()).Returns(contract);
        _currentUser.UserId.Returns(Guid.NewGuid());
        var handler = CreateHandler();
        var command = new CreateMilestoneCommand(contract.Id, "Milestone 2");

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _milestoneRepository.Received(1).AddAsync(
            Arg.Is<Milestone>(m =>
                m.ProjectId == contract.ProjectId &&
                m.TenantId == contract.TenantId &&
                m.ContractId == contract.Id &&
                m.Title == "Milestone 2"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenContractDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var missingContractId = Guid.NewGuid();
        _contractRepository.GetByIdAsync(missingContractId, Arg.Any<CancellationToken>()).Returns((Contract?)null);
        var handler = CreateHandler();
        var command = new CreateMilestoneCommand(missingContractId, "Milestone 2");

        // Act
        var act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
