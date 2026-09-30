using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// User enquiry about a vehicle listing.
/// </summary>
public class Enquiry : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ListingId { get; set; }

    public VehicleListing? Listing { get; set; }

    public Guid? SellerId { get; set; }

    public Seller? Seller { get; set; }

    public Guid? DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public EnquiryType EnquiryType { get; set; } = EnquiryType.General;

    public string Subject { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public EnquiryStatus Status { get; set; } = EnquiryStatus.New;

    public DateTime? ClosedAt { get; set; }
}
