using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for vehicle type operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VehicleTypesController : ControllerBase
{
    private readonly IVehicleTypeService _service;
    private readonly ILogger<VehicleTypesController> _logger;

    public VehicleTypesController(IVehicleTypeService service, ILogger<VehicleTypesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all vehicle types with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleTypeDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VehicleTypeDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, search, cancellationToken);
        return Ok(ApiResponse<PagedResult<VehicleTypeDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get vehicle type by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VehicleTypeDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Vehicle type not found."));

        return Ok(ApiResponse<VehicleTypeDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new vehicle type.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VehicleTypeDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VehicleTypeDto>>> Create(
        [FromBody] CreateVehicleTypeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<VehicleTypeDto>.SuccessResponse(result, "Vehicle type created successfully."));
    }

    /// <summary>
    /// Update an existing vehicle type.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleTypeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VehicleTypeDto>>> Update(
        Guid id,
        [FromBody] UpdateVehicleTypeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<VehicleTypeDto>.SuccessResponse(result, "Vehicle type updated successfully."));
    }

    /// <summary>
    /// Delete a vehicle type.
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
