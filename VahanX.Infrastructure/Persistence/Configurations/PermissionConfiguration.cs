using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Permission.
/// </summary>
public class PermissionConfiguration : BaseEntityConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);
        builder.ToTable("Permissions");
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
        builder.Property(e => e.Module).HasMaxLength(100).IsRequired();
        builder.Property(e => e.IsSystemDefined).HasDefaultValue(true);
        builder.HasIndex(e => e.Name).IsUnique();
    }
}
