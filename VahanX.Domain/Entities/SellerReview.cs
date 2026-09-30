using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Review of a seller.
/// </summary>
public class SellerReview : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid SellerId { get; set; }

    public Seller? Seller { get; set; }

    public int Rating { get; set; }

    public string? Title { get; set; }

    public string? Comment { get; set; }

    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
}
