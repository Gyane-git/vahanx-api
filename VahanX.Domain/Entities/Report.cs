using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// User-submitted report for problematic content.
/// </summary>
public class Report : BaseEntity
{
    public Guid ReporterUserId { get; set; }
    public TargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid ReportReasonId { get; set; }
    public string? Description { get; set; }
    public ModerationReportStatus Status { get; set; } = ModerationReportStatus.Pending;
    public ReportPriority Priority { get; set; } = ReportPriority.Normal;
    public Guid? AssignedToUserId { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNote { get; set; }
}
