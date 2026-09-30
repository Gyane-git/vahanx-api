using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Conversation.
/// </summary>
public class ConversationConfiguration : BaseEntityConfiguration<Conversation>
{
    public override void Configure(EntityTypeBuilder<Conversation> builder)
    {
        base.Configure(builder);

        builder.ToTable("Conversations");

        builder.Property(e => e.ConversationType)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.HasOne(e => e.Enquiry)
            .WithMany()
            .HasForeignKey(e => e.EnquiryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Listing)
            .WithMany()
            .HasForeignKey(e => e.ListingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.LastMessageAt);
        builder.HasIndex(e => e.Status);
    }
}
