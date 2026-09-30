using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Review of a dealer.
/// </summary>
public class DealerReview : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid DealerId { get; set; }

    public Dealer? Dealer { get; set; }

    public int Rating { get; set; }

    public string? Title { get; set; }

    public string? Comment { get; set; }

    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
}
