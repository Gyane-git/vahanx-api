using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Search vehicle response DTO.
/// </summary>
public class SearchVehicleResponse
{
    public Guid ListingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string VehicleName { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = string.Empty;
    public int ManufactureYear { get; set; }
    public Condition Condition { get; set; }
    public string? LocationName { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public DateTime PublishedAt { get; set; }
}

/// <summary>
/// Search query parameters.
/// </summary>
public class SearchVehicleRequest
{
    public string? Keyword { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? ModelId { get; set; }
    public Guid? VariantId { get; set; }
    public Guid? VehicleTypeId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? FuelTypeId { get; set; }
    public Guid? TransmissionTypeId { get; set; }
    public Guid? BodyTypeId { get; set; }
    public Guid? LocationId { get; set; }
    public Guid? SellerId { get; set; }
    public Guid? DealerId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinMileage { get; set; }
    public int? MaxMileage { get; set; }
    public int? ManufactureYear { get; set; }
    public SortOrder SortBy { get; set; } = SortOrder.Newest;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
