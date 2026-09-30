using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for NotificationPreference.
/// </summary>
public class NotificationPreferenceConfiguration : BaseEntityConfiguration<NotificationPreference>
{
    public override void Configure(EntityTypeBuilder<NotificationPreference> builder)
    {
        base.Configure(builder);

        builder.ToTable("NotificationPreferences");

        builder.Property(e => e.NotificationType)
            .IsRequired();

        builder.HasIndex(e => e.UserId);

        builder.HasIndex(e => new { e.UserId, e.NotificationType })
            .IsUnique();
    }
}
