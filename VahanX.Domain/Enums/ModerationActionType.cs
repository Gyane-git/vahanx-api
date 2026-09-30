namespace VahanX.Domain.Enums;

/// <summary>
/// Types of moderation actions that can be performed.
/// </summary>
public enum ModerationActionType
{
    Warn = 0,
    Hide = 1,
    Unhide = 2,
    Reject = 3,
    Remove = 4,
    Suspend = 5,
    Unsuspend = 6,
    Restrict = 7,
    Unrestrict = 8,
    Approve = 9,
    Escalate = 10
}
