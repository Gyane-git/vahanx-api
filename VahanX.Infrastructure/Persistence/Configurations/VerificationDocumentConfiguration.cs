using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for VerificationDocument.
/// </summary>
public class VerificationDocumentConfiguration : BaseEntityConfiguration<VerificationDocument>
{
    public override void Configure(EntityTypeBuilder<VerificationDocument> builder)
    {
        base.Configure(builder);

        builder.ToTable("VerificationDocuments");

        builder.Property(e => e.DocumentType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.DocumentNumber)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.DocumentMediaReference)
            .HasMaxLength(2048)
            .IsRequired(false);

        builder.Property(e => e.Status)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.HasOne(e => e.Verification)
            .WithMany(e => e.Documents)
            .HasForeignKey(e => e.VerificationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.VerificationId);
    }
}
