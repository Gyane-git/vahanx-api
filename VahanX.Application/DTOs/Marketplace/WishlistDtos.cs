namespace VahanX.Application.DTOs.Marketplace;

/// <summary>
/// Wishlist response DTO.
/// </summary>
public class WishlistResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<WishlistItemResponse> Items { get; set; } = [];
}

/// <summary>
/// Wishlist item response DTO.
/// </summary>
public class WishlistItemResponse
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public string VehicleName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? PrimaryImageUrl { get; set; }
    public DateTime AddedAt { get; set; }
}
