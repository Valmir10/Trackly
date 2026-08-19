using FluentAssertions;
using NSubstitute;
using Trackly.Application.Common.Interfaces;
using Trackly.Application.Features.Contracts.Queries.GetProjectContracts;
using Trackly.Domain.Entities;

namespace Trackly.Application.UnitTests.Features.Contracts.Queries.GetProjectContracts;

public class GetProjectContractsQueryHandlerTests
{
    private readonly IContractRepository _contractRepository = Substitute.For<IContractRepository>();

    private GetProjectContractsQueryHandler CreateHandler() => new(_contractRepository);

    [Fact]
    public async Task Handle_ReturnsContractsForTheProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var contract = Contract.Create(Guid.NewGuid(), projectId, "Meridian SOW", Guid.NewGuid());
        _contractRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Contract> { contract });
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectContractsQuery(projectId), CancellationToken.None);

        // Assert
        var dto = result.Should().ContainSingle().Subject;
        dto.Id.Should().Be(contract.Id);
        dto.ProjectId.Should().Be(projectId);
        dto.Title.Should().Be("Meridian SOW");
    }

    [Fact]
    public async Task Handle_WithNoContracts_ReturnsEmptyList()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        _contractRepository.GetByProjectIdAsync(projectId, Arg.Any<CancellationToken>()).Returns(new List<Contract>());
        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetProjectContractsQuery(projectId), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
