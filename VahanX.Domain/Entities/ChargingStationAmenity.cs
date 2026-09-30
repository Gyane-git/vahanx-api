using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Amenity available at a charging station.
/// </summary>
public class ChargingStationAmenity : BaseEntity
{
    public Guid ChargingStationId { get; set; }

    public ChargingStation? ChargingStation { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsAvailable { get; set; } = true;
}
