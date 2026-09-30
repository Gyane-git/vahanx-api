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
    Publish = 11,
    Unpublish = 12,
    Suspend = 13,
    Unsuspend = 14,
    PasswordChanged = 15,
    RoleChanged = 16,
    PermissionChanged = 17,
    PaymentCreated = 18,
    PaymentSucceeded = 19,
    RefundCreated = 20,
    SubscriptionActivated = 21,
    SubscriptionCancelled = 22,
    ModerationAction = 23,
    VerificationAction = 24,
    CMSPublished = 25,
    CMSUnpublished = 26,
    Other = 99
}
