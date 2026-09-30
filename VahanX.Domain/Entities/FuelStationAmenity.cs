using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Amenity available at a fuel station.
/// </summary>
public class FuelStationAmenity : BaseEntity
{
    public Guid FuelStationId { get; set; }

    public FuelStation? FuelStation { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsAvailable { get; set; } = true;
}
