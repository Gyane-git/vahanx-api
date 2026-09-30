namespace VahanX.Domain.Enums;

/// <summary>
/// Charging station pricing type.
/// </summary>
public enum ChargingPricingType
{
    PerKwh = 0,
    PerMinute = 1,
    PerSession = 2,
    FlatRate = 3
}
