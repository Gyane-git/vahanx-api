namespace VahanX.Domain.Enums;

/// <summary>
/// Status lifecycle for moderation cases.
/// </summary>
public enum ModerationCaseStatus
{
    Open = 0,
    InProgress = 1,
    Resolved = 2,
    Escalated = 3,
    Closed = 4
}
