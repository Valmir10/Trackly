using FluentAssertions;
using Trackly.Domain.Entities;

namespace Trackly.Domain.UnitTests.Entities;

public class ContractTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidCreatedById = Guid.NewGuid();

    [Fact]
    public void Create_WithValidData_ReturnsContractWithCorrectProperties()
    {
        // Act
        var contract = Contract.Create(ValidTenantId, ValidProjectId, "Meridian SOW", ValidCreatedById);

        // Assert
        contract.Id.Should().NotBe(Guid.Empty);
        contract.TenantId.Should().Be(ValidTenantId);
        contract.ProjectId.Should().Be(ValidProjectId);
        contract.Title.Should().Be("Meridian SOW");
        contract.PdfObjectKey.Should().BeNull();
        contract.CreatedById.Should().Be(ValidCreatedById);
        contract.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        var act = () => Contract.Create(Guid.Empty, ValidProjectId, "Meridian SOW", ValidCreatedById);
        act.Should().Throw<ArgumentException>().WithParameterName("tenantId");
    }

    [Fact]
    public void Create_WithEmptyProjectId_ThrowsArgumentException()
    {
        var act = () => Contract.Create(ValidTenantId, Guid.Empty, "Meridian SOW", ValidCreatedById);
        act.Should().Throw<ArgumentException>().WithParameterName("projectId");
    }

    [Fact]
    public void Create_WithBlankTitle_ThrowsArgumentException()
    {
        var act = () => Contract.Create(ValidTenantId, ValidProjectId, "   ", ValidCreatedById);
        act.Should().Throw<ArgumentException>().WithParameterName("title");
    }

    [Fact]
    public void Create_WithEmptyCreatedById_ThrowsArgumentException()
    {
        var act = () => Contract.Create(ValidTenantId, ValidProjectId, "Meridian SOW", Guid.Empty);
        act.Should().Throw<ArgumentException>().WithParameterName("createdById");
    }
}
