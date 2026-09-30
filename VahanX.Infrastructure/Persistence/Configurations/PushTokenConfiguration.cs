using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PushToken.
/// </summary>
public class PushTokenConfiguration : BaseEntityConfiguration<PushToken>
{
    public override void Configure(EntityTypeBuilder<PushToken> builder)
    {
        base.Configure(builder);

        builder.ToTable("PushTokens");

        builder.Property(e => e.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.DeviceId)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(e => e.Platform)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Token);
    }
}
