using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Pricing for a charging station.
/// Historical records are preserved.
/// </summary>
public class ChargingStationPrice : BaseEntity
{
    public Guid ChargingStationId { get; set; }

    public ChargingStation? ChargingStation { get; set; }

    public ChargingConnectorType? ConnectorType { get; set; }

    public ChargingPricingType PricingType { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = "NPR";

    public string Unit { get; set; } = string.Empty;

    public DateTime EffectiveFrom { get; set; }

    public DateTime? EffectiveTo { get; set; }

    public bool IsActive { get; set; } = true;
}
