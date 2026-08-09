using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence.Configurations;

public sealed class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
{
    public void Configure(EntityTypeBuilder<Meeting> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Title).IsRequired().HasMaxLength(200);
        builder.Property(m => m.Notes).IsRequired().HasMaxLength(20000);

        builder.HasIndex(m => m.TenantId);
        // Supports a project's meetings list, ordered by when they happen.
        builder.HasIndex(m => new { m.ProjectId, m.ScheduledAt });

        builder.HasOne<Project>().WithMany().HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
