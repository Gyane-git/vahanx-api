using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for InspectionItem.
/// </summary>
public class InspectionItemConfiguration : BaseEntityConfiguration<InspectionItem>
{
    public override void Configure(EntityTypeBuilder<InspectionItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("InspectionItems");

        builder.Property(e => e.Category)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.ItemName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Score)
            .HasPrecision(5, 2)
            .IsRequired(false);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.Condition)
            .IsRequired();

        builder.HasOne(e => e.Inspection)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.InspectionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.InspectionId);
    }
}
