namespace VahanX.Domain.Enums;

/// <summary>
/// Fuel type available at a fuel station.
/// Station-specific inventory, distinct from Vehicle Core FuelType.
/// </summary>
public enum StationFuelType
{
    Petrol = 0,
    Diesel = 1,
    Kerosene = 2,
    LPG = 3,
    CNG = 4,
    Other = 99
}
