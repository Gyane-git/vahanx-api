using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Listing media response DTO.
/// </summary>
public class ListingMediaResponse
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public string? Caption { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Create listing media request.
/// </summary>
public class CreateListingMediaRequest
{
    public string MediaUrl { get; set; } = string.Empty;
    public MediaType MediaType { get; set; } = MediaType.Image;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }
    public string? Caption { get; set; }
}

/// <summary>
/// Update listing media request.
/// </summary>
public class UpdateListingMediaRequest
{
    public string? MediaUrl { get; set; }
    public MediaType? MediaType { get; set; }
    public int? DisplayOrder { get; set; }
    public string? Caption { get; set; }
}
