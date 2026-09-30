namespace VahanX.Domain.Enums;

/// <summary>
/// Status lifecycle for moderation reports.
/// </summary>
public enum ModerationReportStatus
{
    Pending = 0,
    UnderReview = 1,
    Resolved = 2,
    Rejected = 3,
    Dismissed = 4,
    Escalated = 5
}
