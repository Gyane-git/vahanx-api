using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// EV charging station.
/// </summary>
public class ChargingStation : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? OperatorName { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? ContactPhone { get; set; }

    public string? ContactEmail { get; set; }

    public string? WebsiteUrl { get; set; }

    public ChargingStationStatus Status { get; set; } = ChargingStationStatus.Active;

    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Unverified;

    public bool Is24Hours { get; set; }

    public ICollection<ChargingStationConnector> Connectors { get; set; } = new List<ChargingStationConnector>();

    public ICollection<ChargingStationAmenity> Amenities { get; set; } = new List<ChargingStationAmenity>();

    public ICollection<ChargingStationAvailability> Availability { get; set; } = new List<ChargingStationAvailability>();

    public ICollection<ChargingStationPrice> Prices { get; set; } = new List<ChargingStationPrice>();
}
