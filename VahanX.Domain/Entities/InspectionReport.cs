using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Generated inspection report.
/// </summary>
public class InspectionReport : BaseEntity
{
    public Guid InspectionId { get; set; }

    public Inspection? Inspection { get; set; }

    public decimal? OverallScore { get; set; }

    public string? Summary { get; set; }

    public string? Recommendations { get; set; }

    public ReportStatus ReportStatus { get; set; } = ReportStatus.Draft;

    public string? ReportReference { get; set; }

    public DateTime? GeneratedAt { get; set; }
}
