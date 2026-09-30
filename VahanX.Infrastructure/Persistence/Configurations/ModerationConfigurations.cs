using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for ReportReason.
/// </summary>
public class ReportReasonConfiguration : BaseEntityConfiguration<ReportReason>
{
    public override void Configure(EntityTypeBuilder<ReportReason> builder)
    {
        base.Configure(builder);
        builder.ToTable("ReportReasons");
        builder.Property(e => e.Code).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500).IsRequired(false);
        builder.HasIndex(e => e.Code).IsUnique();
        builder.HasIndex(e => e.TargetType);
        builder.HasIndex(e => e.IsActive);
    }
}

/// <summary>
/// Entity configuration for Report.
/// </summary>
public class ReportConfiguration : BaseEntityConfiguration<Report>
{
    public override void Configure(EntityTypeBuilder<Report> builder)
    {
        base.Configure(builder);
        builder.ToTable("Reports");
        builder.Property(e => e.Description).HasMaxLength(2000).IsRequired(false);
        builder.Property(e => e.ResolutionNote).HasMaxLength(2000).IsRequired(false);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Priority);
        builder.HasIndex(e => new { e.TargetType, e.TargetId });
        builder.HasIndex(e => e.ReporterUserId);
        builder.HasIndex(e => e.AssignedToUserId);
        builder.HasIndex(e => e.CreatedAt);
    }
}

/// <summary>
/// Entity configuration for ModerationCase.
/// </summary>
public class ModerationCaseConfiguration : BaseEntityConfiguration<ModerationCase>
{
    public override void Configure(EntityTypeBuilder<ModerationCase> builder)
    {
        base.Configure(builder);
        builder.ToTable("ModerationCases");
        builder.Property(e => e.ResolutionNote).HasMaxLength(2000).IsRequired(false);
        builder.HasOne<Report>().WithMany().HasForeignKey(e => e.ReportId).OnDelete(DeleteBehavior.SetNull);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Priority);
        builder.HasIndex(e => e.AssignedToUserId);
        builder.HasIndex(e => new { e.TargetType, e.TargetId });
    }
}

/// <summary>
/// Entity configuration for ModerationAction.
/// </summary>
public class ModerationActionConfiguration : BaseEntityConfiguration<ModerationAction>
{
    public override void Configure(EntityTypeBuilder<ModerationAction> builder)
    {
        base.Configure(builder);
        builder.ToTable("ModerationActions");
        builder.Property(e => e.Reason).HasMaxLength(500).IsRequired();
        builder.Property(e => e.Notes).HasMaxLength(2000).IsRequired(false);
        builder.HasIndex(e => e.ModerationCaseId);
        builder.HasIndex(e => e.ActionType);
        builder.HasIndex(e => e.PerformedByUserId);
    }
}

/// <summary>
/// Entity configuration for ModerationHistory.
/// </summary>
public class ModerationHistoryConfiguration : BaseEntityConfiguration<ModerationHistory>
{
    public override void Configure(EntityTypeBuilder<ModerationHistory> builder)
    {
        base.Configure(builder);
        builder.ToTable("ModerationHistory");
        builder.Property(e => e.Reason).HasMaxLength(500).IsRequired();
        builder.HasIndex(e => e.ModerationCaseId);
        builder.HasIndex(e => e.ChangedAt);
    }
}

/// <summary>
/// Entity configuration for UserRestriction.
/// </summary>
public class UserRestrictionConfiguration : BaseEntityConfiguration<UserRestriction>
{
    public override void Configure(EntityTypeBuilder<UserRestriction> builder)
    {
        base.Configure(builder);
        builder.ToTable("UserRestrictions");
        builder.Property(e => e.Reason).HasMaxLength(500).IsRequired();
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.EndsAt);
    }
}
