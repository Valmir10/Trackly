using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trackly.Domain.Entities;

namespace Trackly.Infrastructure.Persistence.Configurations;

public sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedNever();

        builder.Property(m => m.Content).IsRequired().HasMaxLength(4000);

        // Supports ChatThread's actual query shape: all messages for a scope
        // (project stream or a ticket's thread), ordered by time.
        builder.HasIndex(m => new { m.ProjectId, m.TicketId, m.CreatedAt });

        builder.HasOne<Project>().WithMany().HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Ticket>().WithMany().HasForeignKey(m => m.TicketId).OnDelete(DeleteBehavior.Restrict);
    }
}
