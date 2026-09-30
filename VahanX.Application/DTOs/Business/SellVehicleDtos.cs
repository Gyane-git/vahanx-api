using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Business;

/// <summary>
/// Sell request response DTO.
/// </summary>
public class SellRequestResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SellVehicleId { get; set; }
    public string VehicleDisplayName { get; set; } = string.Empty;
    public ContactPreference PreferredContactMethod { get; set; }
    public string? PreferredContactTime { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public SellRequestStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}

/// <summary>
/// Create sell request.
/// </summary>
public class CreateSellRequestRequest
{
    public ContactPreference PreferredContactMethod { get; set; } = ContactPreference.Phone;
    public string? PreferredContactTime { get; set; }
    public Guid? LocationId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Update sell request.
/// </summary>
public class UpdateSellRequestRequest
{
    public ContactPreference? PreferredContactMethod { get; set; }
    public string? PreferredContactTime { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Sell vehicle response DTO.
/// </summary>
public class SellVehicleResponse
{
    public Guid Id { get; set; }
    public Guid SellRequestId { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid VehicleTypeId { get; set; }
    public string VehicleTypeName { get; set; } = string.Empty;
    public Guid BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public Guid ModelId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public Guid? VariantId { get; set; }
    public string? VariantName { get; set; }
    public int ManufactureYear { get; set; }
    public int? RegistrationYear { get; set; }
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = string.Empty;
    public Guid FuelTypeId { get; set; }
    public string FuelTypeName { get; set; } = string.Empty;
    public Guid? TransmissionTypeId { get; set; }
    public string? TransmissionTypeName { get; set; }
    public Condition Condition { get; set; }
    public decimal? AskingPrice { get; set; }
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
}

/// <summary>
/// Create sell vehicle request.
/// </summary>
public class CreateSellVehicleRequest
{
    public Guid? VehicleId { get; set; }
    public Guid VehicleTypeId { get; set; }
    public Guid BrandId { get; set; }
    public Guid ModelId { get; set; }
    public Guid? VariantId { get; set; }
    public int ManufactureYear { get; set; }
    public int? RegistrationYear { get; set; }
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = "km";
    public Guid FuelTypeId { get; set; }
    public Guid? TransmissionTypeId { get; set; }
    public Condition Condition { get; set; } = Condition.Used;
    public decimal? AskingPrice { get; set; }
    public string? Description { get; set; }
    public Guid? LocationId { get; set; }
}

/// <summary>
/// Sell offer response DTO.
/// </summary>
public class SellOfferResponse
{
    public Guid Id { get; set; }
    public Guid SellRequestId { get; set; }
    public Guid? OfferedByUserId { get; set; }
    public decimal OfferedAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime? ValidUntil { get; set; }
    public string? Notes { get; set; }
    public SellOfferStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
}

/// <summary>
/// Create sell offer request.
/// </summary>
public class CreateSellOfferRequest
{
    public decimal OfferedAmount { get; set; }
    public string Currency { get; set; } = "NPR";
    public DateTime? ValidUntil { get; set; }
    public string? Notes { get; set; }
}
