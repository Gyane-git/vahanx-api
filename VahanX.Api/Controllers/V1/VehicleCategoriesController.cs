using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for vehicle category operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VehicleCategoriesController : ControllerBase
{
    private readonly IVehicleCategoryService _service;
    private readonly ILogger<VehicleCategoriesController> _logger;

    public VehicleCategoriesController(IVehicleCategoryService service, ILogger<VehicleCategoriesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all vehicle categories with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleCategoryDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VehicleCategoryDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? vehicleTypeId = null,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, vehicleTypeId, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<VehicleCategoryDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get vehicle category by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VehicleCategoryDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Vehicle category not found."));

        return Ok(ApiResponse<VehicleCategoryDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new vehicle category.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VehicleCategoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VehicleCategoryDto>>> Create(
        [FromBody] CreateVehicleCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<VehicleCategoryDto>.SuccessResponse(result, "Vehicle category created successfully."));
    }

    /// <summary>
    /// Update an existing vehicle category.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleCategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VehicleCategoryDto>>> Update(
        Guid id,
        [FromBody] UpdateVehicleCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<VehicleCategoryDto>.SuccessResponse(result, "Vehicle category updated successfully."));
    }

    /// <summary>
    /// Delete a vehicle category.
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
