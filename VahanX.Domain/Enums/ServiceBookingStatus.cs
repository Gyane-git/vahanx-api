namespace VahanX.Domain.Enums;

/// <summary>
/// Service booking lifecycle status.
/// </summary>
public enum ServiceBookingStatus
{
    Requested = 0,
    Pending = 1,
    Confirmed = 2,
    Rescheduled = 3,
    InProgress = 4,
    Completed = 5,
    Cancelled = 6,
    Rejected = 7,
    NoShow = 8
}
