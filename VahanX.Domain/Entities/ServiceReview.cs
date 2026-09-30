using VahanX.Domain.Common;
using VahanX.Domain.Enums;

namespace VahanX.Domain.Entities;

/// <summary>
/// Review of a completed service booking.
/// Follows the existing review architecture pattern.
/// </summary>
public class ServiceReview : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid ServiceCenterId { get; set; }

    public ServiceCenter? ServiceCenter { get; set; }

    public Guid? ServiceCenterBranchId { get; set; }

    public ServiceCenterBranch? ServiceCenterBranch { get; set; }

    public Guid ServiceBookingId { get; set; }

    public ServiceBooking? ServiceBooking { get; set; }

    public int Rating { get; set; }

    public string? Title { get; set; }

    public string? Comment { get; set; }

    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
}
