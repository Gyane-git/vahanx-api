using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Fuel availability status for a fuel station.
/// </summary>
public class FuelStationAvailability : BaseEntity
{
    public Guid FuelStationId { get; set; }

    public FuelStation? FuelStation { get; set; }

    public Guid? FuelStationFuelTypeId { get; set; }

    public FuelStationFuelType? FuelType { get; set; }

    public FuelStationAvailabilityStatus Status { get; set; } = FuelStationAvailabilityStatus.Available;

    public DateTime? UpdatedAt { get; set; }
}
