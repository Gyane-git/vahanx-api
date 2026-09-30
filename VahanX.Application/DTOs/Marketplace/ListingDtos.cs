using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Vehicle listing response DTO.
/// </summary>
public class ListingResponse
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public Guid? SellerId { get; set; }
    public string? SellerName { get; set; }
    public Guid? DealerId { get; set; }
    public string? DealerName { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = string.Empty;
    public int ManufactureYear { get; set; }
    public int? RegistrationYear { get; set; }
    public Condition Condition { get; set; }
    public ListingStatus Status { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public ContactPreference ContactPreference { get; set; }
    public bool IsNegotiable { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Listing summary response DTO for list views.
/// </summary>
public class ListingSummaryResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string VehicleName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = string.Empty;
    public int ManufactureYear { get; set; }
    public Condition Condition { get; set; }
    public ListingStatus Status { get; set; }
    public string? LocationName { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Create listing request.
/// </summary>
public class CreateListingRequest
{
    public Guid VehicleId { get; set; }
    public Guid? SellerId { get; set; }
    public Guid? DealerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "NPR";
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = "km";
    public int ManufactureYear { get; set; }
    public int? RegistrationYear { get; set; }
    public Condition Condition { get; set; } = Condition.Used;
    public Guid? LocationId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public ContactPreference ContactPreference { get; set; } = ContactPreference.Phone;
    public bool IsNegotiable { get; set; }
}

/// <summary>
/// Update listing request.
/// </summary>
public class UpdateListingRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "NPR";
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = "km";
    public int ManufactureYear { get; set; }
    public int? RegistrationYear { get; set; }
    public Condition Condition { get; set; }
    public Guid? LocationId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public ContactPreference ContactPreference { get; set; }
    public bool IsNegotiable { get; set; }
}
