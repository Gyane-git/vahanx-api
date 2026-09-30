using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for vehicle listing operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ListingsController : ControllerBase
{
    private readonly IListingService _service;
    private readonly ILogger<ListingsController> _logger;

    public ListingsController(IListingService service, ILogger<ListingsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all listings with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ListingResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ListingResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? vehicleId = null,
        [FromQuery] Guid? sellerId = null,
        [FromQuery] Guid? dealerId = null,
        [FromQuery] ListingStatus? status = null,
        [FromQuery] Guid? locationId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int? minMileage = null,
        [FromQuery] int? maxMileage = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, vehicleId, sellerId, dealerId, status, locationId, minPrice, maxPrice, minMileage, maxMileage, cancellationToken);
        return Ok(ApiResponse<PagedResult<ListingResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get listing by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Listing not found."));

        return Ok(ApiResponse<ListingResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new listing in Draft status.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> Create(
        [FromBody] CreateListingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ListingResponse>.SuccessResponse(result, "Listing created successfully."));
    }

    /// <summary>
    /// Update an existing listing.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> Update(
        Guid id,
        [FromBody] UpdateListingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<ListingResponse>.SuccessResponse(result, "Listing updated successfully."));
    }

    /// <summary>
    /// Delete a listing.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Submit a listing for review (Draft → PendingReview).
    /// </summary>
    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> Submit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.SubmitAsync(id, cancellationToken);
        return Ok(ApiResponse<ListingResponse>.SuccessResponse(result, "Listing submitted for review."));
    }

    /// <summary>
    /// Publish a listing (PendingReview/Draft → Published).
    /// </summary>
    [HttpPost("{id:guid}/publish")]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> Publish(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.PublishAsync(id, cancellationToken);
        return Ok(ApiResponse<ListingResponse>.SuccessResponse(result, "Listing published successfully."));
    }

    /// <summary>
    /// Pause a published listing.
    /// </summary>
    [HttpPost("{id:guid}/pause")]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> Pause(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.PauseAsync(id, cancellationToken);
        return Ok(ApiResponse<ListingResponse>.SuccessResponse(result, "Listing paused successfully."));
    }

    /// <summary>
    /// Mark a listing as sold.
    /// </summary>
    [HttpPost("{id:guid}/mark-sold")]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> MarkSold(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.MarkSoldAsync(id, cancellationToken);
        return Ok(ApiResponse<ListingResponse>.SuccessResponse(result, "Listing marked as sold."));
    }

    /// <summary>
    /// Archive a listing.
    /// </summary>
    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(typeof(ApiResponse<ListingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ListingResponse>>> Archive(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.ArchiveAsync(id, cancellationToken);
        return Ok(ApiResponse<ListingResponse>.SuccessResponse(result, "Listing archived successfully."));
    }
}
