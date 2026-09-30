using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for User.
/// </summary>
public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.ToTable("Users");
        builder.Property(e => e.Email).HasMaxLength(256).IsRequired();
        builder.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();
        builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.IsActive);
    }
}
