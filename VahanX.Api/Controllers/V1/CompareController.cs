using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for compare list operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class CompareController : ControllerBase
{
    private readonly ICompareService _service;
    private readonly ILogger<CompareController> _logger;

    public CompareController(ICompareService service, ILogger<CompareController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get the current user's compare list.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<CompareResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<CompareResponse>>> GetCompareList(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetCompareListAsync(userId, cancellationToken);
        return Ok(ApiResponse<CompareResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Add a listing to the compare list.
    /// </summary>
    [HttpPost("items/{listingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CompareResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<CompareResponse>>> AddItem(
        [FromQuery] Guid userId,
        Guid listingId,
        CancellationToken cancellationToken)
    {
        var result = await _service.AddItemAsync(userId, listingId, cancellationToken);
        return Ok(ApiResponse<CompareResponse>.SuccessResponse(result, "Item added to compare list."));
    }

    /// <summary>
    /// Remove a listing from the compare list.
    /// </summary>
    [HttpDelete("items/{listingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CompareResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CompareResponse>>> RemoveItem(
        [FromQuery] Guid userId,
        Guid listingId,
        CancellationToken cancellationToken)
    {
        var result = await _service.RemoveItemAsync(userId, listingId, cancellationToken);
        return Ok(ApiResponse<CompareResponse>.SuccessResponse(result, "Item removed from compare list."));
    }
}
