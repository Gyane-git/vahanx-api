using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Marketplace;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for seller operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SellersController : ControllerBase
{
    private readonly ISellerService _service;
    private readonly ILogger<SellersController> _logger;

    public SellersController(ISellerService service, ILogger<SellersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all sellers with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SellerResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<SellerResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<SellerResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get seller by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SellerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SellerResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Seller not found."));

        return Ok(ApiResponse<SellerResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new seller.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SellerResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<SellerResponse>>> Create(
        [FromBody] CreateSellerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<SellerResponse>.SuccessResponse(result, "Seller created successfully."));
    }

    /// <summary>
    /// Update an existing seller.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SellerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<SellerResponse>>> Update(
        Guid id,
        [FromBody] UpdateSellerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<SellerResponse>.SuccessResponse(result, "Seller updated successfully."));
    }

    /// <summary>
    /// Delete a seller.
    /// /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Get listings for a specific seller.
    /// </summary>
    [HttpGet("{id:guid}/listings")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ListingResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PagedResult<ListingResponse>>>> GetSellerListings(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetSellerListingsAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ListingResponse>>.SuccessResponse(result));
    }
}
