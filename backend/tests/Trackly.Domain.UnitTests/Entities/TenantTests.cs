using FluentAssertions;
using Trackly.Domain.Entities;
using Trackly.Domain.Enums;
using Trackly.Domain.Events;

namespace Trackly.Domain.UnitTests.Entities;

public class TenantTests
{
    // -------------------------------------------------------
    // Tenant.Create
    // -------------------------------------------------------

    [Fact]
    public void Create_WithValidData_ReturnsTenantWithCorrectProperties()
    {
        // Arrange
        var name = "Acme Corp";
        var slug = "acme-corp";

        // Act
        var tenant = Tenant.Create(name, slug);

        // Assert
        tenant.Id.Should().NotBe(Guid.Empty);
        tenant.Name.Should().Be("Acme Corp");
        tenant.Slug.Should().Be("acme-corp");
        tenant.Tier.Should().Be(SubscriptionTier.Free);
        tenant.StripeCustomerId.Should().BeNull();
        tenant.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_TrimsNameWhitespace()
    {
        // Arrange + Act
        var tenant = Tenant.Create("  Acme Corp  ", "acme-corp");

        // Assert
        tenant.Name.Should().Be("Acme Corp");
    }

    [Fact]
    public void Create_NormalizesSlugToLowercase()
    {
        // Arrange + Act
        var tenant = Tenant.Create("Acme Corp", "ACME-CORP");

        // Assert
        tenant.Slug.Should().Be("acme-corp");
    }

    // -------------------------------------------------------
    // Tenant.Create — domänhändelse
    // -------------------------------------------------------

    [Fact]
    public void Create_RaisesTenantCreatedEvent()
    {
        // Arrange + Act
        var tenant = Tenant.Create("Acme Corp", "acme-corp");

        // Assert
        tenant.DomainEvents.Should().ContainSingle(e => e is TenantCreatedEvent);

        var domainEvent = tenant.DomainEvents.Single() as TenantCreatedEvent;
        domainEvent!.TenantId.Should().Be(tenant.Id);
        domainEvent.Name.Should().Be("Acme Corp");
        domainEvent.Slug.Should().Be("acme-corp");
    }

    // -------------------------------------------------------
    // Tenant.Create — ogiltiga indata
    // -------------------------------------------------------

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Tenant.Create("", "acme-corp");

        // Assert
        act.Should().Throw<ArgumentException>()
            .Which.Message.Should().Contain("name");
    }

    [Fact]
    public void Create_WithWhitespaceName_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Tenant.Create("   ", "acme-corp");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithSlugContainingSpaces_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Tenant.Create("Acme Corp", "acme corp");

        // Assert
        act.Should().Throw<ArgumentException>()
            .Which.Message.Should().Contain("Slug");
    }

    [Fact]
    public void Create_WithSlugStartingWithHyphen_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Tenant.Create("Acme Corp", "-acme-corp");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithSlugEndingWithHyphen_ThrowsArgumentException()
    {
        // Arrange
        Action act = () => Tenant.Create("Acme Corp", "acme-corp-");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // UpdateName
    // -------------------------------------------------------

    [Fact]
    public void UpdateName_WithValidName_UpdatesName()
    {
        // Arrange
        var tenant = Tenant.Create("Acme Corp", "acme-corp");

        // Act
        tenant.UpdateName("Acme Inc");

        // Assert
        tenant.Name.Should().Be("Acme Inc");
    }

    [Fact]
    public void UpdateName_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var tenant = Tenant.Create("Acme Corp", "acme-corp");
        Action act = () => tenant.UpdateName("");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    // -------------------------------------------------------
    // SetSubscriptionTier
    // -------------------------------------------------------

    [Fact]
    public void SetSubscriptionTier_UpdatesTier()
    {
        // Arrange
        var tenant = Tenant.Create("Acme Corp", "acme-corp");

        // Act
        tenant.SetSubscriptionTier(SubscriptionTier.Pro);

        // Assert
        tenant.Tier.Should().Be(SubscriptionTier.Pro);
    }
}
