using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence.Configurations;

public sealed class ApprovalConfiguration : IEntityTypeConfiguration<Approval>
{
    public void Configure(EntityTypeBuilder<Approval> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.HasIndex(a => a.TenantId);
        // One approval per milestone — the idempotency check in
        // ApproveMilestoneCommandHandler relies on this being unique.
        builder.HasIndex(a => a.MilestoneId).IsUnique();

        builder.HasOne<Milestone>().WithMany().HasForeignKey(a => a.MilestoneId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<ClientRoomAccess>().WithMany().HasForeignKey(a => a.ClientRoomAccessId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Project>().WithMany().HasForeignKey(a => a.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
