using Trackly.Domain.Common;
using Trackly.Domain.Enums;
using Trackly.Domain.Events;

namespace Trackly.Domain.Entities;

public sealed class Tenant : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public SubscriptionTier Tier { get; private set; }
    public string? StripeCustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Tenant() { }

    public static Tenant Create(string name, string slug)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tenant name cannot be empty.", nameof(name));

        if (!IsValidSlug(slug))
            throw new ArgumentException("Slug must contain only lowercase letters, numbers and hyphens.", nameof(slug));

        var tenant = new Tenant
        {
            Name = name.Trim(),
            Slug = slug.ToLowerInvariant(),
            Tier = SubscriptionTier.Free,
            CreatedAt = DateTime.UtcNow
        };

        tenant.AddDomainEvent(new TenantCreatedEvent(tenant.Id, tenant.Name, tenant.Slug));
        return tenant;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tenant name cannot be empty.", nameof(name));

        Name = name.Trim();
    }

    public void SetSubscriptionTier(SubscriptionTier tier)
    {
        Tier = tier;
    }

    public void SetStripeCustomerId(string stripeCustomerId)
    {
        StripeCustomerId = stripeCustomerId;
    }

    private static bool IsValidSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return false;
        return slug.All(c => char.IsLetterOrDigit(c) || c == '-')
            && !slug.StartsWith('-')
            && !slug.EndsWith('-');
    }
}
