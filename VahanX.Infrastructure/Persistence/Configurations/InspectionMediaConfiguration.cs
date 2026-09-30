using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for InspectionMedia.
/// </summary>
public class InspectionMediaConfiguration : BaseEntityConfiguration<InspectionMedia>
{
    public override void Configure(EntityTypeBuilder<InspectionMedia> builder)
    {
        base.Configure(builder);

        builder.ToTable("InspectionMedia");

        builder.Property(e => e.MediaReference)
            .HasMaxLength(2048)
            .IsRequired();

        builder.Property(e => e.Caption)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(e => e.MediaType)
            .IsRequired();

        builder.HasOne(e => e.Inspection)
            .WithMany(e => e.Media)
            .HasForeignKey(e => e.InspectionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.InspectionItem)
            .WithMany()
            .HasForeignKey(e => e.InspectionItemId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.InspectionId);
    }
}
