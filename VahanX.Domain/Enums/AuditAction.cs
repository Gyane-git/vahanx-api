namespace VahanX.Domain.Enums;

/// <summary>
/// Types of audit actions that can be tracked.
/// </summary>
public enum AuditAction
{
    Create = 1,
    Update = 2,
    Delete = 3,
    View = 4,
    Login = 5,
    Logout = 6,
    Export = 7,
    Import = 8,
    Approve = 9,
    Reject = 10,
    Other = 99
}
