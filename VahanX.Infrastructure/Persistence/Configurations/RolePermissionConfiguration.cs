using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for RolePermission.
/// </summary>
public class RolePermissionConfiguration : BaseEntityConfiguration<RolePermission>
{
    public override void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        base.Configure(builder);
        builder.ToTable("RolePermissions");
        builder.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();
        builder.HasIndex(e => e.RoleId);
        builder.HasIndex(e => e.PermissionId);
    }
}
