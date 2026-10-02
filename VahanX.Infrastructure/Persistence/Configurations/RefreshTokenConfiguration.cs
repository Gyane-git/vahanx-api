using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for RefreshToken.
/// </summary>
public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);
        builder.ToTable("RefreshTokens");
        builder.Property(e => e.TokenHash).HasMaxLength(128).IsRequired();
        builder.Property(e => e.ReplacedByTokenHash).HasMaxLength(128).IsRequired(false);
        builder.Property(e => e.IpAddress).HasMaxLength(64).IsRequired(false);
        builder.Property(e => e.UserAgent).HasMaxLength(512).IsRequired(false);
        builder.Ignore(e => e.IsActive);
        builder.HasIndex(e => e.TokenHash).IsUnique();
        builder.HasIndex(e => e.UserId);
    }
}
