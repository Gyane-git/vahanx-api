namespace VahanX.Domain.Enums;

/// <summary>
/// Vehicle registration status.
/// </summary>
public enum RegistrationStatus
{
    Active = 0,
    Expired = 1,
    Suspended = 2,
    Cancelled = 3,
    Transferred = 4,
    Unknown = 99
}
