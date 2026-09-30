using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Media item for a vehicle listing.
/// Uses URL/storage reference architecture, not binary storage in SQL Server.
/// Ready for future S3, Azure Blob, Cloudinary integration.
/// </summary>
public class ListingMedia : BaseEntity
{
    public Guid ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public string MediaUrl { get; set; } = string.Empty;

    public MediaType MediaType { get; set; } = MediaType.Image;

    public int DisplayOrder { get; set; }

    public bool IsPrimary { get; set; }

    public string? Caption { get; set; }
}
