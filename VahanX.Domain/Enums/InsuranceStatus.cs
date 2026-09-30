namespace VahanX.Domain.Enums;

/// <summary>
/// Insurance policy status.
/// </summary>
public enum InsuranceStatus
{
    Active = 0,
    Expired = 1,
    Cancelled = 2,
    Lapsed = 3,
    Unknown = 99
}
