using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for UserRole.
/// </summary>
public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    public override void Configure(EntityTypeBuilder<UserRole> builder)
    {
        base.Configure(builder);
        builder.ToTable("UserRoles");
        builder.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.RoleId);
    }
}
