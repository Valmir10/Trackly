using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class MeetingTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidProjectId = Guid.NewGuid();
    private static readonly Guid ValidCreatedById = Guid.NewGuid();
    private static readonly DateTime ValidScheduledAt = new(2026, 8, 20, 14, 0, 0, DateTimeKind.Utc);

    // -------------------------------------------------------
    // Meeting.Create
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsMeetingWithCorrectProperties()
    {
        // Act
        var meeting = Meeting.Create(ValidTenantId, ValidProjectId, "Sprint Planning 14", ValidScheduledAt, ValidCreatedById);

        // Assert
        meeting.Id.Should().NotBe(Guid.Empty);
        meeting.TenantId.Should().Be(ValidTenantId);
        meeting.ProjectId.Should().Be(ValidProjectId);
        meeting.Title.Should().Be("Sprint Planning 14");
        meeting.ScheduledAt.Should().Be(ValidScheduledAt);
        meeting.Notes.Should().BeEmpty();
        meeting.CreatedById.Should().Be(ValidCreatedById);
        meeting.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        meeting.UpdatedAt.Should().Be(meeting.CreatedAt);
    }

    [Fact]
    public void Create_RaisesMeetingCreatedEvent()
    {
        // Act
        var meeting = Meeting.Create(ValidTenantId, ValidProjectId, "Sprint Planning 14", ValidScheduledAt, ValidCreatedById);

        // Assert
        meeting.DomainEvents.Should().ContainSingle(e => e is MeetingCreatedEvent);

        var domainEvent = meeting.DomainEvents.Single() as MeetingCreatedEvent;
        domainEvent!.MeetingId.Should().Be(meeting.Id);
        domainEvent.ProjectId.Should().Be(ValidProjectId);
        domainEvent.Title.Should().Be("Sprint Planning 14");
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        // Act
        var act = () => Meeting.Create(Guid.Empty, ValidProjectId, "Sprint Planning 14", ValidScheduledAt, ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("tenantId");
    }

    [Fact]
    public void Create_WithEmptyProjectId_ThrowsArgumentException()
    {
        // Act
        var act = () => Meeting.Create(ValidTenantId, Guid.Empty, "Sprint Planning 14", ValidScheduledAt, ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("projectId");
    }

    [Fact]
    public void Create_WithBlankTitle_ThrowsArgumentException()
    {
        // Act
        var act = () => Meeting.Create(ValidTenantId, ValidProjectId, "   ", ValidScheduledAt, ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("title");
    }

    [Fact]
    public void Create_WithEmptyCreatedById_ThrowsArgumentException()
    {
        // Act
        var act = () => Meeting.Create(ValidTenantId, ValidProjectId, "Sprint Planning 14", ValidScheduledAt, Guid.Empty);

        // Assert
        act.Should().Throw<ArgumentException>().WithParameterName("createdById");
    }

    // -------------------------------------------------------
    // Meeting.UpdateNotes
    // -------------------------------------------------------

    [Fact]
    public void UpdateNotes_SetsNotesAndBumpsUpdatedAt()
    {
        // Arrange
        var meeting = Meeting.Create(ValidTenantId, ValidProjectId, "Sprint Planning 14", ValidScheduledAt, ValidCreatedById);
        var createdAt = meeting.CreatedAt;

        // Act
        meeting.UpdateNotes("Discussed the roadmap. >a1b2 Ship v2 by Friday.");

        // Assert
        meeting.Notes.Should().Be("Discussed the roadmap. >a1b2 Ship v2 by Friday.");
        meeting.UpdatedAt.Should().BeOnOrAfter(createdAt);
    }
}
