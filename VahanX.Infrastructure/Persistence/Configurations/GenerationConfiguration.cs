using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Generation.
/// </summary>
public class GenerationConfiguration : BaseEntityConfiguration<Generation>
{
    public override void Configure(EntityTypeBuilder<Generation> builder)
    {
        base.Configure(builder);

        builder.ToTable("Generations");

        builder.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(e => e.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.StartYear)
            .IsRequired();

        builder.Property(e => e.EndYear)
            .IsRequired(false);

        builder.Property(e => e.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.Property(e => e.DisplayOrder)
            .HasDefaultValue(0);

        builder.HasOne(e => e.Model)
            .WithMany(e => e.Generations)
            .HasForeignKey(e => e.ModelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.ModelId);

        builder.HasIndex(e => new { e.ModelId, e.Code })
            .IsUnique();

        builder.HasIndex(e => e.StartYear);

        builder.HasIndex(e => e.IsActive);
    }
}
