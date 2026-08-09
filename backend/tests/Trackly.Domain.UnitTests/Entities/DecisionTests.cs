using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class DecisionTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidMeetingId = Guid.NewGuid();
    private static readonly Guid ValidCreatedById = Guid.NewGuid();

    // -------------------------------------------------------
    // Decision.Create
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsDecisionWithCorrectProperties()
    {
        // Act
        var decision = Decision.Create(ValidTenantId, ValidProjectId, ValidMeetingId, "Ship v2 by Friday", ValidCreatedById);

        // Assert
        decision.Id.Should().NotBe(Guid.Empty);
        decision.TenantId.Should().Be(ValidTenantId);
        decision.ProjectId.Should().Be(ValidProjectId);
        decision.MeetingId.Should().Be(ValidMeetingId);
        decision.Text.Should().Be("Ship v2 by Friday");
        decision.CreatedById.Should().Be(ValidCreatedById);
        decision.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_RaisesDecisionCreatedEvent()
    {
        // Act
        var decision = Decision.Create(ValidTenantId, ValidProjectId, ValidMeetingId, "Ship v2 by Friday", ValidCreatedById);

        // Assert
        decision.DomainEvents.Should().ContainSingle(e => e is DecisionCreatedEvent);

        var domainEvent = decision.DomainEvents.Single() as DecisionCreatedEvent;
        domainEvent!.DecisionId.Should().Be(decision.Id);
        domainEvent.MeetingId.Should().Be(ValidMeetingId);
        domainEvent.ProjectId.Should().Be(ValidProjectId);
        domainEvent.Text.Should().Be("Ship v2 by Friday");
        domainEvent.CreatedById.Should().Be(ValidCreatedById);
        domainEvent.CreatedAt.Should().Be(decision.CreatedAt);
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        // Act
        var act = () => Decision.Create(Guid.Empty, ValidProjectId, ValidMeetingId, "Ship v2 by Friday", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("tenantId");
    }

    [Fact]
    public void Create_WithEmptyProjectId_ThrowsArgumentException()
    {
        // Act
        var act = () => Decision.Create(ValidTenantId, Guid.Empty, ValidMeetingId, "Ship v2 by Friday", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("projectId");
    }

    [Fact]
    public void Create_WithEmptyMeetingId_ThrowsArgumentException()
    {
        // Act
        var act = () => Decision.Create(ValidTenantId, ValidProjectId, Guid.Empty, "Ship v2 by Friday", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("meetingId");
    }

    [Fact]
    public void Create_WithBlankText_ThrowsArgumentException()
    {
        // Act
        var act = () => Decision.Create(ValidTenantId, ValidProjectId, ValidMeetingId, "   ", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("text");
    }

    [Fact]
    public void Create_WithEmptyCreatedById_ThrowsArgumentException()
    {
        // Act
        var act = () => Decision.Create(ValidTenantId, ValidProjectId, ValidMeetingId, "Ship v2 by Friday", Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("createdById");
    }
}
