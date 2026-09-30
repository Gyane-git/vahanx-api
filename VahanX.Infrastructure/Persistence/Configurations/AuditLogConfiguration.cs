using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AuditLog.
/// </summary>
public class AuditLogConfiguration : BaseEntityConfiguration<AuditLog>
{
    public override void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        base.Configure(builder);

        builder.ToTable("AuditLogs");

        builder.Property(e => e.ActorUserId)
            .IsRequired(false);

        builder.Property(e => e.Action)
            .IsRequired();

        builder.Property(e => e.EntityType)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(e => e.EntityId)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(e => e.OldValues)
            .IsRequired(false);

        builder.Property(e => e.NewValues)
            .IsRequired(false);

        builder.Property(e => e.Changes)
            .IsRequired(false);

        builder.Property(e => e.IpAddress)
            .HasMaxLength(45)
            .IsRequired(false);

        builder.Property(e => e.UserAgent)
            .HasMaxLength(512)
            .IsRequired(false);

        builder.Property(e => e.CorrelationId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Timestamp)
            .IsRequired();

        builder.Property(e => e.Result)
            .IsRequired();

        builder.Property(e => e.FailureReason)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasIndex(e => e.ActorUserId);
        builder.HasIndex(e => e.EntityType);
        builder.HasIndex(e => e.EntityId);
        builder.HasIndex(e => e.Timestamp);
        builder.HasIndex(e => e.Action);
        builder.HasIndex(e => e.CorrelationId);
    }
}
