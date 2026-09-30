using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Fuel type available at a fuel station.
/// Station-specific inventory, distinct from Vehicle Core FuelType.
/// </summary>
public class FuelStationFuelType : BaseEntity
{
    public Guid FuelStationId { get; set; }

    public FuelStation? FuelStation { get; set; }

    public StationFuelType FuelType { get; set; }

    public bool IsAvailable { get; set; } = true;
}
