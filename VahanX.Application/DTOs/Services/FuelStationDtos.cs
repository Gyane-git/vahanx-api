using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Services;

/// <summary>
/// Fuel station list DTO.
/// </summary>
public class FuelStationListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? OperatorName { get; set; }
    public FuelStationStatus Status { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public bool Is24Hours { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? DistanceKm { get; set; }
}

/// <summary>
/// Fuel station details DTO.
/// </summary>
public class FuelStationDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? OperatorName { get; set; }
    public string? Description { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? WebsiteUrl { get; set; }
    public bool Is24Hours { get; set; }
    public FuelStationStatus Status { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

/// <summary>
/// Fuel station fuel type response DTO.
/// </summary>
public class FuelStationFuelTypeResponse
{
    public Guid Id { get; set; }
    public Guid FuelStationId { get; set; }
    public StationFuelType FuelType { get; set; }
    public bool IsAvailable { get; set; }
}

/// <summary>
/// Fuel station amenity response DTO.
/// </summary>
public class FuelStationAmenityResponse
{
    public Guid Id { get; set; }
    public Guid FuelStationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }
}

/// <summary>
/// Fuel station availability response DTO.
/// </summary>
public class FuelStationAvailabilityResponse
{
    public Guid Id { get; set; }
    public Guid FuelStationId { get; set; }
    public Guid? FuelStationFuelTypeId { get; set; }
    public StationFuelType? FuelType { get; set; }
    public FuelStationAvailabilityStatus Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Fuel station price response DTO.
/// </summary>
public class FuelStationPriceResponse
{
    public Guid Id { get; set; }
    public Guid FuelStationId { get; set; }
    public Guid? FuelStationFuelTypeId { get; set; }
    public StationFuelType? FuelType { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsCurrent { get; set; }
}

/// <summary>
/// Fuel station working hour response DTO.
/// </summary>
public class FuelStationWorkingHourResponse
{
    public Guid Id { get; set; }
    public Guid FuelStationId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public bool IsClosed { get; set; }
}
