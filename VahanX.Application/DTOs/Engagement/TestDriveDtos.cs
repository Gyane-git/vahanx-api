using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Engagement;

/// <summary>
/// Test drive response DTO.
/// </summary>
public class TestDriveResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ListingId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public Guid? SellerId { get; set; }
    public string? SellerName { get; set; }
    public Guid? DealerId { get; set; }
    public string? DealerName { get; set; }
    public DateTime RequestedDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public string? Notes { get; set; }
    public TestDriveStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}

/// <summary>
/// Create test drive request.
/// </summary>
public class CreateTestDriveRequest
{
    public Guid ListingId { get; set; }
    public DateTime RequestedDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid? LocationId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Update test drive request.
/// </summary>
public class UpdateTestDriveRequest
{
    public DateTime? RequestedDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid? LocationId { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Reschedule test drive request.
/// </summary>
public class RescheduleTestDriveRequest
{
    public DateTime NewStartTime { get; set; }
    public DateTime NewEndTime { get; set; }
}
