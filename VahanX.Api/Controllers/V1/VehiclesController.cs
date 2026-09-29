using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.VehicleCore;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for vehicle operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _service;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(IVehicleService service, ILogger<VehiclesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all vehicles with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VehicleDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VehicleDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? variantId = null,
        [FromQuery] Guid? fuelTypeId = null,
        [FromQuery] Guid? bodyTypeId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, variantId, fuelTypeId, bodyTypeId, cancellationToken);
        return Ok(ApiResponse<PagedResult<VehicleDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get vehicle by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Vehicle not found."));

        return Ok(ApiResponse<VehicleDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new vehicle.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VehicleDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> Create(
        [FromBody] CreateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<VehicleDto>.SuccessResponse(result, "Vehicle created successfully."));
    }

    /// <summary>
    /// Update an existing vehicle.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VehicleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VehicleDto>>> Update(
        Guid id,
        [FromBody] UpdateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<VehicleDto>.SuccessResponse(result, "Vehicle updated successfully."));
    }

    /// <summary>
    /// Delete a vehicle.
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
