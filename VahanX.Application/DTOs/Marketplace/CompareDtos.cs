namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Compare list response DTO.
/// </summary>
public class CompareResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<CompareItemResponse> Items { get; set; } = [];
}

/// <summary>
/// Compare item response DTO.
/// </summary>
public class CompareItemResponse
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public string VehicleName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Mileage { get; set; }
    public string MileageUnit { get; set; } = string.Empty;
    public int ManufactureYear { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public DateTime AddedAt { get; set; }
}
