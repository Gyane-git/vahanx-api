namespace VahanX.Domain.Enums;

/// <summary>
/// Test drive lifecycle status.
/// </summary>
public enum TestDriveStatus
{
    Requested = 0,
    Pending = 1,
    Confirmed = 2,
    Rescheduled = 3,
    Completed = 4,
    Cancelled = 5,
    Rejected = 6,
    NoShow = 7
}
