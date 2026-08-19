using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class ApprovalTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidMilestoneId = Guid.NewGuid();
    private static readonly Guid ValidClientRoomAccessId = Guid.NewGuid();

    [Fact]
    public void Create_WithValidData_ReturnsApprovalWithCorrectProperties()
    {
        // Act
        var approval = Approval.Create(ValidTenantId, ValidProjectId, ValidMilestoneId, ValidClientRoomAccessId);

        // Assert
        approval.Id.Should().NotBe(Guid.Empty);
        approval.TenantId.Should().Be(ValidTenantId);
        approval.ProjectId.Should().Be(ValidProjectId);
        approval.MilestoneId.Should().Be(ValidMilestoneId);
        approval.ClientRoomAccessId.Should().Be(ValidClientRoomAccessId);
        approval.ApprovedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_RaisesMilestoneApprovedEvent()
    {
        // Act
        var approval = Approval.Create(ValidTenantId, ValidProjectId, ValidMilestoneId, ValidClientRoomAccessId);

        // Assert
        approval.DomainEvents.Should().ContainSingle(e => e is MilestoneApprovedEvent);

        var domainEvent = approval.DomainEvents.Single() as MilestoneApprovedEvent;
        domainEvent!.ApprovalId.Should().Be(approval.Id);
        domainEvent.MilestoneId.Should().Be(ValidMilestoneId);
        domainEvent.ProjectId.Should().Be(ValidProjectId);
        domainEvent.TenantId.Should().Be(ValidTenantId);
        domainEvent.ApprovedAt.Should().Be(approval.ApprovedAt);
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        var act = () => Approval.Create(Guid.Empty, ValidProjectId, ValidMilestoneId, ValidClientRoomAccessId);
        act.Should().Throw<ArgumentException>().WithParameterName("tenantId");
    }

    [Fact]
    public void Create_WithEmptyProjectId_ThrowsArgumentException()
    {
        var act = () => Approval.Create(ValidTenantId, Guid.Empty, ValidMilestoneId, ValidClientRoomAccessId);
        act.Should().Throw<ArgumentException>().WithParameterName("projectId");
    }

    [Fact]
    public void Create_WithEmptyMilestoneId_ThrowsArgumentException()
    {
        var act = () => Approval.Create(ValidTenantId, ValidProjectId, Guid.Empty, ValidClientRoomAccessId);
        act.Should().Throw<ArgumentException>().WithParameterName("milestoneId");
    }

    [Fact]
    public void Create_WithEmptyClientRoomAccessId_ThrowsArgumentException()
    {
        var act = () => Approval.Create(ValidTenantId, ValidProjectId, ValidMilestoneId, Guid.Empty);
        act.Should().Throw<ArgumentException>().WithParameterName("clientRoomAccessId");
    }
}
