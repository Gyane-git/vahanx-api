using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for NotificationTemplate.
/// </summary>
public class NotificationTemplateConfiguration : BaseEntityConfiguration<NotificationTemplate>
{
    public override void Configure(EntityTypeBuilder<NotificationTemplate> builder)
    {
        base.Configure(builder);

        builder.ToTable("NotificationTemplates");

        builder.Property(e => e.Code)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.TitleTemplate)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.MessageTemplate)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(e => e.Language)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.Channel)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(e => new { e.Code, e.Channel, e.Language })
            .IsUnique();
    }
}
