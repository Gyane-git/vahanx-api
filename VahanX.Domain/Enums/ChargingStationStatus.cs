namespace VahanX.Domain.Enums;

/// <summary>
/// Charging station operational status.
/// </summary>
public enum ChargingStationStatus
{
    Active = 0,
    Inactive = 1,
    Maintenance = 2,
    TemporarilyUnavailable = 3,
    Closed = 4
}
