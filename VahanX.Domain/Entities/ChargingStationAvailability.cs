using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Availability status for a charging station or connector.
/// </summary>
public class ChargingStationAvailability : BaseEntity
{
    public Guid ChargingStationId { get; set; }

    public ChargingStation? ChargingStation { get; set; }

    public Guid? ConnectorId { get; set; }

    public ChargingStationConnector? Connector { get; set; }

    public ChargingAvailabilityStatus Status { get; set; } = ChargingAvailabilityStatus.Available;

    public DateTime? AvailableFrom { get; set; }

    public DateTime? AvailableUntil { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
