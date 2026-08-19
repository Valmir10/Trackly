using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence.Configurations;

public sealed class ClientRoomAccessConfiguration : IEntityTypeConfiguration<ClientRoomAccess>
{
    public void Configure(EntityTypeBuilder<ClientRoomAccess> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.TokenHash).IsRequired().HasMaxLength(200);
        // Looked up by hash on every request — the auth handler's whole job
        // depends on this being unique and indexed.
        builder.HasIndex(a => a.TokenHash).IsUnique();

        builder.HasIndex(a => a.ProjectId);

        builder.HasOne<Project>().WithMany().HasForeignKey(a => a.ProjectId).OnDelete(DeleteBehavior.Restrict);
    }
}
