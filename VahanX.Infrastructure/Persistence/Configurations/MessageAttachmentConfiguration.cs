using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for MessageAttachment.
/// </summary>
public class MessageAttachmentConfiguration : BaseEntityConfiguration<MessageAttachment>
{
    public override void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        base.Configure(builder);

        builder.ToTable("MessageAttachments");

        builder.Property(e => e.MediaReference)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(e => e.FileName)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.ContentType)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.HasOne(e => e.Message)
            .WithMany(e => e.Attachments)
            .HasForeignKey(e => e.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.MessageId);
    }
}
