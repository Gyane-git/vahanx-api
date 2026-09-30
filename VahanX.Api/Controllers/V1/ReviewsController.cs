using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for review operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}")]
[ApiVersion("1.0")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _service;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(IReviewService service, ILogger<ReviewsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get reviews for a vehicle.
    /// </summary>
    [HttpGet("vehicles/{vehicleId:guid}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleReviewResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VehicleReviewResponse>>>> GetVehicleReviews(
        Guid vehicleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetVehicleReviewsAsync(vehicleId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<VehicleReviewResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a review for a vehicle.
    /// </summary>
    [HttpPost("vehicles/{vehicleId:guid}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<VehicleReviewResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VehicleReviewResponse>>> CreateVehicleReview(
        Guid vehicleId,
        [FromQuery] Guid userId,
        [FromBody] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateVehicleReviewAsync(vehicleId, userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetVehicleReviewById), new { id = result.Id }, ApiResponse<VehicleReviewResponse>.SuccessResponse(result, "Review created successfully."));
    }

    /// <summary>
    /// Get a vehicle review by ID.
    /// </summary>
    [HttpGet("reviews/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VehicleReviewResponse>>> GetVehicleReviewById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetVehicleReviewByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Review not found."));

        return Ok(ApiResponse<VehicleReviewResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Update a vehicle review.
    /// </summary>
    [HttpPut("reviews/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<VehicleReviewResponse>>> UpdateVehicleReview(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateVehicleReviewAsync(id, userId, request, cancellationToken);
        return Ok(ApiResponse<VehicleReviewResponse>.SuccessResponse(result, "Review updated successfully."));
    }

    /// <summary>
    /// Delete a review.
    /// </summary>
    [HttpDelete("reviews/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteReview(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        await _service.DeleteReviewAsync(id, userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Report a review.
    /// </summary>
    [HttpPost("reviews/{id:guid}/report")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReportReview(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] ReportReviewRequest request,
        CancellationToken cancellationToken)
    {
        await _service.ReportReviewAsync(id, userId, request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Get reviews for a seller.
    /// </summary>
    [HttpGet("sellers/{sellerId:guid}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SellerReviewResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<SellerReviewResponse>>>> GetSellerReviews(
        Guid sellerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetSellerReviewsAsync(sellerId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<SellerReviewResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a review for a seller.
    /// </summary>
    [HttpPost("sellers/{sellerId:guid}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<SellerReviewResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<SellerReviewResponse>>> CreateSellerReview(
        Guid sellerId,
        [FromQuery] Guid userId,
        [FromBody] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateSellerReviewAsync(sellerId, userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetSellerReviews), new { sellerId }, ApiResponse<SellerReviewResponse>.SuccessResponse(result, "Review created successfully."));
    }

    /// <summary>
    /// Get reviews for a dealer.
    /// </summary>
    [HttpGet("dealers/{dealerId:guid}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DealerReviewResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<DealerReviewResponse>>>> GetDealerReviews(
        Guid dealerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetDealerReviewsAsync(dealerId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<DealerReviewResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a review for a dealer.
    /// </summary>
    [HttpPost("dealers/{dealerId:guid}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<DealerReviewResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<DealerReviewResponse>>> CreateDealerReview(
        Guid dealerId,
        [FromQuery] Guid userId,
        [FromBody] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateDealerReviewAsync(dealerId, userId, request, cancellationToken);
        return CreatedAtAction(nameof(GetDealerReviews), new { dealerId }, ApiResponse<DealerReviewResponse>.SuccessResponse(result, "Review created successfully."));
    }

    /// <summary>
    /// Get rating summary for a vehicle.
    /// </summary>
    [HttpGet("vehicles/{vehicleId:guid}/rating-summary")]
    [ProducesResponseType(typeof(ApiResponse<RatingSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RatingSummaryResponse>>> GetVehicleRatingSummary(
        Guid vehicleId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetVehicleRatingSummaryAsync(vehicleId, cancellationToken);
        return Ok(ApiResponse<RatingSummaryResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get rating summary for a seller.
    /// </summary>
    [HttpGet("sellers/{sellerId:guid}/rating-summary")]
    [ProducesResponseType(typeof(ApiResponse<RatingSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RatingSummaryResponse>>> GetSellerRatingSummary(
        Guid sellerId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetSellerRatingSummaryAsync(sellerId, cancellationToken);
        return Ok(ApiResponse<RatingSummaryResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get rating summary for a dealer.
    /// </summary>
    [HttpGet("dealers/{dealerId:guid}/rating-summary")]
    [ProducesResponseType(typeof(ApiResponse<RatingSummaryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<RatingSummaryResponse>>> GetDealerRatingSummary(
        Guid dealerId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetDealerRatingSummaryAsync(dealerId, cancellationToken);
        return Ok(ApiResponse<RatingSummaryResponse>.SuccessResponse(result));
    }
}
