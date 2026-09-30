using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for wishlist operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _service;
    private readonly ILogger<WishlistController> _logger;

    public WishlistController(IWishlistService service, ILogger<WishlistController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get the current user's wishlist.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<WishlistResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<WishlistResponse>>> GetWishlist(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetWishlistAsync(userId, cancellationToken);
        return Ok(ApiResponse<WishlistResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Add a listing to the user's wishlist.
    /// </summary>
    [HttpPost("items/{listingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<WishlistResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<WishlistResponse>>> AddItem(
        [FromQuery] Guid userId,
        Guid listingId,
        CancellationToken cancellationToken)
    {
        var result = await _service.AddItemAsync(userId, listingId, cancellationToken);
        return Ok(ApiResponse<WishlistResponse>.SuccessResponse(result, "Item added to wishlist."));
    }

    /// <summary>
    /// Remove a listing from the user's wishlist.
    /// </summary>
    [HttpDelete("items/{listingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<WishlistResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<WishlistResponse>>> RemoveItem(
        [FromQuery] Guid userId,
        Guid listingId,
        CancellationToken cancellationToken)
    {
        var result = await _service.RemoveItemAsync(userId, listingId, cancellationToken);
        return Ok(ApiResponse<WishlistResponse>.SuccessResponse(result, "Item removed from wishlist."));
    }
}
