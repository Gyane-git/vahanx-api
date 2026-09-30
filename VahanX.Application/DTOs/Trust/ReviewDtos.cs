using VahanX.Domain.Enums;

namespace VahanX.Application.DTOs.Trust;

/// <summary>
/// Vehicle review response DTO.
/// </summary>
public class VehicleReviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? ListingId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public ReviewStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Seller review response DTO.
/// </summary>
public class SellerReviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SellerId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public ReviewStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Dealer review response DTO.
/// </summary>
public class DealerReviewResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid DealerId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public ReviewStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Create review request.
/// </summary>
public class CreateReviewRequest
{
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
}

/// <summary>
/// Update review request.
/// </summary>
public class UpdateReviewRequest
{
    public int? Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
}

/// <summary>
/// Rating summary response DTO.
/// </summary>
public class RatingSummaryResponse
{
    public decimal AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int Rating1Count { get; set; }
    public int Rating2Count { get; set; }
    public int Rating3Count { get; set; }
    public int Rating4Count { get; set; }
    public int Rating5Count { get; set; }
}

/// <summary>
/// Report review request.
/// </summary>
public class ReportReviewRequest
{
    public string Reason { get; set; } = string.Empty;
    public string? Description { get; set; }
}
