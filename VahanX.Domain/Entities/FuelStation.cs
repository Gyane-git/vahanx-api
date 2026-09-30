using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Fuel station.
/// </summary>
public class FuelStation : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? OperatorName { get; set; }

    public string? Description { get; set; }

    public Guid? LocationId { get; set; }

    public Location? Location { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? ContactPhone { get; set; }

    public string? ContactEmail { get; set; }

    public string? WebsiteUrl { get; set; }

    public bool Is24Hours { get; set; }

    public FuelStationStatus Status { get; set; } = FuelStationStatus.Active;

    public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Unverified;

    public ICollection<FuelStationFuelType> FuelTypes { get; set; } = new List<FuelStationFuelType>();

    public ICollection<FuelStationAmenity> Amenities { get; set; } = new List<FuelStationAmenity>();

    public ICollection<FuelStationAvailability> Availability { get; set; } = new List<FuelStationAvailability>();

    public ICollection<FuelStationPrice> Prices { get; set; } = new List<FuelStationPrice>();

    public ICollection<FuelStationWorkingHour> WorkingHours { get; set; } = new List<FuelStationWorkingHour>();
}
