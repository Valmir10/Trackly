using FluentAssertions;
using Trackly.Domain.Entities;

namespace Trackly.Domain.UnitTests.Entities;

public class MilestoneTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidContractId = Guid.NewGuid();
    private static readonly Guid ValidCreatedById = Guid.NewGuid();

    [Fact]
    public void Create_WithValidData_ReturnsMilestoneWithCorrectProperties()
    {
        // Act
        var milestone = Milestone.Create(ValidTenantId, ValidProjectId, ValidContractId, "Milestone 2", ValidCreatedById);

        // Assert
        milestone.Id.Should().NotBe(Guid.Empty);
        milestone.TenantId.Should().Be(ValidTenantId);
        milestone.ProjectId.Should().Be(ValidProjectId);
        milestone.ContractId.Should().Be(ValidContractId);
        milestone.Title.Should().Be("Milestone 2");
        milestone.CreatedById.Should().Be(ValidCreatedById);
        milestone.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        var act = () => Milestone.Create(Guid.Empty, ValidProjectId, ValidContractId, "Milestone 2", ValidCreatedById);
        act.Should().Throw<ArgumentException>().WithParameterName("tenantId");
    }

    [Fact]
    public void Create_WithEmptyProjectId_ThrowsArgumentException()
    {
        var act = () => Milestone.Create(ValidTenantId, Guid.Empty, ValidContractId, "Milestone 2", ValidCreatedById);
        act.Should().Throw<ArgumentException>().WithParameterName("projectId");
    }

    [Fact]
    public void Create_WithEmptyContractId_ThrowsArgumentException()
    {
        var act = () => Milestone.Create(ValidTenantId, ValidProjectId, Guid.Empty, "Milestone 2", ValidCreatedById);
        act.Should().Throw<ArgumentException>().WithParameterName("contractId");
    }

    [Fact]
    public void Create_WithBlankTitle_ThrowsArgumentException()
    {
        var act = () => Milestone.Create(ValidTenantId, ValidProjectId, ValidContractId, "   ", ValidCreatedById);
        act.Should().Throw<ArgumentException>().WithParameterName("title");
    }

    [Fact]
    public void Create_WithEmptyCreatedById_ThrowsArgumentException()
    {
        var act = () => Milestone.Create(ValidTenantId, ValidProjectId, ValidContractId, "Milestone 2", Guid.Empty);
        act.Should().Throw<ArgumentException>().WithParameterName("createdById");
    }
}
