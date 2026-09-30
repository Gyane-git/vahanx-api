using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Centralized moderation case that may be linked to a report.
/// </summary>
public class ModerationCase : BaseEntity
{
    public Guid? ReportId { get; set; }
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public ModerationCaseStatus Status { get; set; } = ModerationCaseStatus.Open;
    public ReportPriority Priority { get; set; } = ReportPriority.Normal;
    public DateTime? StartedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public ModerationResolutionType? ResolutionType { get; set; }
    public string? ResolutionNote { get; set; }
}
