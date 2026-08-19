using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence.Configurations;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(4000);
        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Priority).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(t => t.TenantId);
        // Supports the Kanban board's actual query shape: all tickets for a
        // project, grouped by status.
        builder.HasIndex(t => new { t.ProjectId, t.Status });

        builder.HasOne<Project>().WithMany().HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Meeting>().WithMany().HasForeignKey(t => t.OriginMeetingId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Milestone>().WithMany().HasForeignKey(t => t.MilestoneId).OnDelete(DeleteBehavior.Restrict);

        // Two independent self-referencing gates — Restrict (not Cascade)
        // on both to avoid multi-path cascade-delete conflicts.
        builder.HasOne<Ticket>().WithMany().HasForeignKey(t => t.BlockedByTicketId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Milestone>().WithMany().HasForeignKey(t => t.BlockedByMilestoneId).OnDelete(DeleteBehavior.Restrict);
    }
}
