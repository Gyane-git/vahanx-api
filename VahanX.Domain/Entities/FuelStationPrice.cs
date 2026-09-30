using VahanX.Domain.Common;

namespace VahanX.Domain.Entities;

/// <summary>
/// Fuel price at a fuel station.
/// Historical records are preserved.
/// </summary>
public class FuelStationPrice : BaseEntity
{
    public Guid FuelStationId { get; set; }

    public FuelStation? FuelStation { get; set; }

    public Guid? FuelStationFuelTypeId { get; set; }

    public FuelStationFuelType? FuelType { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = "NPR";

    public string Unit { get; set; } = string.Empty;

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public bool IsCurrent { get; set; } = true;
}
