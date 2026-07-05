using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class ProjectTests
{
    private static readonly Guid ValidTenantId = Guid.NewGuid();
    private static readonly Guid ValidCreatedById = Guid.NewGuid();

    // -------------------------------------------------------
    // Project.Create
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsProjectWithCorrectProperties()
    {
        // Act
        var project = Project.Create(ValidTenantId, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);

        // Assert
        project.Id.Should().NotBe(Guid.Empty);
        project.TenantId.Should().Be(ValidTenantId);
        project.Name.Should().Be("Frontend redesign");
        project.Color.Should().Be("var(--tp-cat-1)");
        project.CreatedById.Should().Be(ValidCreatedById);
        project.Description.Should().BeNull();
        project.IsArchived.Should().BeFalse();
        project.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_TrimsNameWhitespace()
    {
        // Act
        var project = Project.Create(ValidTenantId, "  Frontend redesign  ", "var(--tp-cat-1)", ValidCreatedById);

        // Assert
        project.Name.Should().Be("Frontend redesign");
    }

    [Fact]
    public void Create_RaisesProjectCreatedEvent()
    {
        // Act
        var project = Project.Create(ValidTenantId, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);

        // Assert
        project.DomainEvents.Should().ContainSingle(e => e is ProjectCreatedEvent);

        var domainEvent = project.DomainEvents.Single() as ProjectCreatedEvent;
        domainEvent!.ProjectId.Should().Be(project.Id);
        domainEvent.TenantId.Should().Be(ValidTenantId);
        domainEvent.Name.Should().Be("Frontend redesign");
    }

    [Fact]
    public void Create_WithEmptyTenantId_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Project.Create(Guid.Empty, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Project.Create(ValidTenantId, "", "var(--tp-cat-1)", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithEmptyColor_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Project.Create(ValidTenantId, "Frontend redesign", "", ValidCreatedById);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // Rename / UpdateDescription
    // -------------------------------------------------------

    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        // Arrange
        var project = Project.Create(ValidTenantId, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);

        // Act
        project.Rename("API v2");

        // Assert
        project.Name.Should().Be("API v2");
    }

    [Fact]
    public void UpdateDescription_SetsDescription()
    {
        // Arrange
        var project = Project.Create(ValidTenantId, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);

        // Act
        project.UpdateDescription("Migrating the app off Tailwind.");

        // Assert
        project.Description.Should().Be("Migrating the app off Tailwind.");
    }

    // -------------------------------------------------------
    // Archive / Restore
    // -------------------------------------------------------

    [Fact]
    public void Archive_SetsArchivedAt()
    {
        // Arrange
        var project = Project.Create(ValidTenantId, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);

        // Act
        project.Archive();

        // Assert
        project.IsArchived.Should().BeTrue();
        project.ArchivedAt.Should().NotBeNull();
    }

    [Fact]
    public void Archive_WhenAlreadyArchived_IsNoOp()
    {
        // Arrange
        var project = Project.Create(ValidTenantId, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);
        project.Archive();
        var firstArchivedAt = project.ArchivedAt;

        // Act
        project.Archive();

        // Assert
        project.ArchivedAt.Should().Be(firstArchivedAt);
    }

    [Fact]
    public void Restore_ClearsArchivedAt()
    {
        // Arrange
        var project = Project.Create(ValidTenantId, "Frontend redesign", "var(--tp-cat-1)", ValidCreatedById);
        project.Archive();

        // Act
        project.Restore();

        // Assert
        project.IsArchived.Should().BeFalse();
        project.ArchivedAt.Should().BeNull();
    }
}
