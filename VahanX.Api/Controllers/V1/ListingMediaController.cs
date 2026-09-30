using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for listing media operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/listings/{listingId:guid}/media")]
[ApiVersion("1.0")]
public class ListingMediaController : ControllerBase
{
    private readonly IListingMediaService _service;
    private readonly ILogger<ListingMediaController> _logger;

    public ListingMediaController(IListingMediaService service, ILogger<ListingMediaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get media for a specific listing.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ListingMediaResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PagedResult<ListingMediaResponse>>>> GetByListing(
        Guid listingId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByListingAsync(listingId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ListingMediaResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Add media to a listing.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ListingMediaResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingMediaResponse>>> Create(
        Guid listingId,
        [FromBody] CreateListingMediaRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(listingId, request, cancellationToken);
        return CreatedAtAction(nameof(GetByListing), new { listingId }, ApiResponse<ListingMediaResponse>.SuccessResponse(result, "Media added successfully."));
    }

    /// <summary>
    /// Update listing media.
    /// </summary>
    [HttpPut("{mediaId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ListingMediaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingMediaResponse>>> Update(
        Guid listingId,
        Guid mediaId,
        [FromBody] UpdateListingMediaRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(listingId, mediaId, request, cancellationToken);
        return Ok(ApiResponse<ListingMediaResponse>.SuccessResponse(result, "Media updated successfully."));
    }

    /// <summary>
    /// Delete listing media.
    /// /// </summary>
    [HttpDelete("{mediaId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid listingId, Guid mediaId, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(listingId, mediaId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Set a media item as primary.
    /// </summary>
    [HttpPost("{mediaId:guid}/primary")]
    [ProducesResponseType(typeof(ApiResponse<ListingMediaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ListingMediaResponse>>> SetPrimary(
        Guid listingId,
        Guid mediaId,
        CancellationToken cancellationToken)
    {
        var result = await _service.SetPrimaryAsync(listingId, mediaId, cancellationToken);
        return Ok(ApiResponse<ListingMediaResponse>.SuccessResponse(result, "Primary media set successfully."));
    }
}
