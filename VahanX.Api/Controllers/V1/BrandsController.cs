using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for brand operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BrandsController : ControllerBase
{
    private readonly IBrandService _service;
    private readonly ILogger<BrandsController> _logger;

    public BrandsController(IBrandService service, ILogger<BrandsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all brands with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<BrandDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<BrandDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<BrandDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get brand by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BrandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BrandDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Brand not found."));

        return Ok(ApiResponse<BrandDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new brand.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BrandDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<BrandDto>>> Create(
        [FromBody] CreateBrandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<BrandDto>.SuccessResponse(result, "Brand created successfully."));
    }

    /// <summary>
    /// Update an existing brand.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<BrandDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<BrandDto>>> Update(
        Guid id,
        [FromBody] UpdateBrandRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<BrandDto>.SuccessResponse(result, "Brand updated successfully."));
    }

    /// <summary>
    /// Delete a brand.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
