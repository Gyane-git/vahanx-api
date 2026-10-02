using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for LoginHistory.
/// </summary>
public class LoginHistoryConfiguration : BaseEntityConfiguration<LoginHistory>
{
    public override void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        base.Configure(builder);
        builder.ToTable("LoginHistories");
        builder.Property(e => e.Email).HasMaxLength(256).IsRequired();
        builder.Property(e => e.FailureReason).HasMaxLength(200).IsRequired(false);
        builder.Property(e => e.IpAddress).HasMaxLength(64).IsRequired(false);
        builder.Property(e => e.UserAgent).HasMaxLength(512).IsRequired(false);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.Timestamp);
    }
}
