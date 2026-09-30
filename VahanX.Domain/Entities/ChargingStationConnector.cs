using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Connector at a charging station.
/// </summary>
public class ChargingStationConnector : BaseEntity
{
    public Guid ChargingStationId { get; set; }

    public ChargingStation? ChargingStation { get; set; }

    public ChargingConnectorType ConnectorType { get; set; }

    public decimal PowerKw { get; set; }

    public decimal? Voltage { get; set; }

    public decimal? Amperage { get; set; }

    public int Quantity { get; set; } = 1;

    public int? AvailableQuantity { get; set; }

    public string? ChargingMode { get; set; }

    public ChargingStationStatus Status { get; set; } = ChargingStationStatus.Active;
}
