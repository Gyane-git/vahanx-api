using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Model.
/// </summary>
public class ModelConfiguration : BaseEntityConfiguration<Model>
{
    public override void Configure(EntityTypeBuilder<Model> builder)
    {
        base.Configure(builder);

        builder.ToTable("Models");

        builder.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasOne(e => e.Brand)
            .WithMany(e => e.Models)
            .HasForeignKey(e => e.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.BrandId);

        builder.HasIndex(e => new { e.BrandId, e.Code })
            .IsUnique();

        builder.HasIndex(e => new { e.BrandId, e.Name })
            .IsUnique();

        builder.HasIndex(e => e.IsActive);
    }
}
