using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence.Configurations;

public sealed class DecisionConfiguration : IEntityTypeConfiguration<Decision>
{
    public void Configure(EntityTypeBuilder<Decision> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).ValueGeneratedNever();

        builder.Property(d => d.Text).IsRequired().HasMaxLength(1000);

        builder.HasIndex(d => d.TenantId);
        builder.HasIndex(d => d.MeetingId);

        builder.HasOne<Meeting>().WithMany().HasForeignKey(d => d.MeetingId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Project>().WithMany().HasForeignKey(d => d.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
