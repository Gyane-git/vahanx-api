using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ConversationParticipant.
/// </summary>
public class ConversationParticipantConfiguration : BaseEntityConfiguration<ConversationParticipant>
{
    public override void Configure(EntityTypeBuilder<ConversationParticipant> builder)
    {
        base.Configure(builder);

        builder.ToTable("ConversationParticipants");

        builder.Property(e => e.JoinedAt)
            .IsRequired();

        builder.HasOne(e => e.Conversation)
            .WithMany(e => e.Participants)
            .HasForeignKey(e => e.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ConversationId);
        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => new { e.ConversationId, e.UserId })
            .IsUnique();
    }
}
