namespace VahanX.Domain.Enums;

/// <summary>
/// Status lifecycle for inspection reports.
/// </summary>
public enum ReportStatus
{
    Draft = 0,
    Generated = 1,
    Finalized = 2,
    Archived = 3
}
