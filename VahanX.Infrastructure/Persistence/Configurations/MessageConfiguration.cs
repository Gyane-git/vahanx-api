using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Message.
/// </summary>
public class MessageConfiguration : BaseEntityConfiguration<Message>
{
    public override void Configure(EntityTypeBuilder<Message> builder)
    {
        base.Configure(builder);

        builder.ToTable("Messages");

        builder.Property(e => e.Content)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(e => e.MessageType)
            .IsRequired();

        builder.Property(e => e.SentAt)
            .IsRequired();

        builder.HasOne(e => e.Conversation)
            .WithMany(e => e.Messages)
            .HasForeignKey(e => e.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ConversationId);
        builder.HasIndex(e => e.SentAt);
    }
}
