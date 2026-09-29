using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for variant operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VariantsController : ControllerBase
{
    private readonly IVariantService _service;
    private readonly ILogger<VariantsController> _logger;

    public VariantsController(IVariantService service, ILogger<VariantsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all variants with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VariantDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VariantDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? generationId = null,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, generationId, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<VariantDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get variant by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VariantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VariantDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Variant not found."));

        return Ok(ApiResponse<VariantDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new variant.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VariantDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VariantDto>>> Create(
        [FromBody] CreateVariantRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<VariantDto>.SuccessResponse(result, "Variant created successfully."));
    }

    /// <summary>
    /// Update an existing variant.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VariantDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VariantDto>>> Update(
        Guid id,
        [FromBody] UpdateVariantRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<VariantDto>.SuccessResponse(result, "Variant updated successfully."));
    }

    /// <summary>
    /// Delete a variant.
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
