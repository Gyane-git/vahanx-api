using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Services;

/// <summary>
/// Charging station list DTO.
/// </summary>
public class ChargingStationListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? OperatorName { get; set; }
    public ChargingStationStatus Status { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public bool Is24Hours { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? DistanceKm { get; set; }
}

/// <summary>
/// Charging station details DTO.
/// </summary>
public class ChargingStationDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? OperatorName { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? WebsiteUrl { get; set; }
    public ChargingStationStatus Status { get; set; }
    public VerificationStatus VerificationStatus { get; set; }
    public bool Is24Hours { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

/// <summary>
/// Charging station connector response DTO.
/// </summary>
public class ChargingStationConnectorResponse
{
    public Guid Id { get; set; }
    public Guid ChargingStationId { get; set; }
    public ChargingConnectorType ConnectorType { get; set; }
    public decimal PowerKw { get; set; }
    public decimal? Voltage { get; set; }
    public decimal? Amperage { get; set; }
    public int Quantity { get; set; }
    public int? AvailableQuantity { get; set; }
    public string? ChargingMode { get; set; }
    public ChargingStationStatus Status { get; set; }
}

/// <summary>
/// Charging station amenity response DTO.
/// </summary>
public class ChargingStationAmenityResponse
{
    public Guid Id { get; set; }
    public Guid ChargingStationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsAvailable { get; set; }
}

/// <summary>
/// Charging station availability response DTO.
/// </summary>
public class ChargingStationAvailabilityResponse
{
    public Guid Id { get; set; }
    public Guid ChargingStationId { get; set; }
    public Guid? ConnectorId { get; set; }
    public ChargingAvailabilityStatus Status { get; set; }
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableUntil { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Charging station price response DTO.
/// </summary>
public class ChargingStationPriceResponse
{
    public Guid Id { get; set; }
    public Guid ChargingStationId { get; set; }
    public ChargingConnectorType? ConnectorType { get; set; }
    public ChargingPricingType PricingType { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; }
}
