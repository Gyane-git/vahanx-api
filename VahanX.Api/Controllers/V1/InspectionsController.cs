using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for inspection operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class InspectionsController : ControllerBase
{
    private readonly IInspectionService _service;
    private readonly ILogger<InspectionsController> _logger;

    public InspectionsController(IInspectionService service, ILogger<InspectionsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all inspections with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<InspectionResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<InspectionResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? vehicleId = null,
        [FromQuery] Guid? listingId = null,
        [FromQuery] InspectionStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, vehicleId, listingId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<InspectionResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get inspection by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<InspectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<InspectionResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Inspection not found."));

        return Ok(ApiResponse<InspectionResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new inspection.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<InspectionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<InspectionResponse>>> Create(
        [FromBody] CreateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<InspectionResponse>.SuccessResponse(result, "Inspection created successfully."));
    }

    /// <summary>
    /// Update an existing inspection.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<InspectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<InspectionResponse>>> Update(
        Guid id,
        [FromBody] UpdateInspectionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<InspectionResponse>.SuccessResponse(result, "Inspection updated successfully."));
    }

    /// <summary>
    /// Delete an inspection.
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
    /// Start an inspection.
    /// </summary>
    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(typeof(ApiResponse<InspectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<InspectionResponse>>> Start(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.StartAsync(id, cancellationToken);
        return Ok(ApiResponse<InspectionResponse>.SuccessResponse(result, "Inspection started successfully."));
    }

    /// <summary>
    /// Complete an inspection.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<InspectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<InspectionResponse>>> Complete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.CompleteAsync(id, cancellationToken);
        return Ok(ApiResponse<InspectionResponse>.SuccessResponse(result, "Inspection completed successfully."));
    }

    /// <summary>
    /// Cancel an inspection.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<InspectionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<InspectionResponse>>> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.CancelAsync(id, cancellationToken);
        return Ok(ApiResponse<InspectionResponse>.SuccessResponse(result, "Inspection cancelled successfully."));
    }

    /// <summary>
    /// Get inspection items.
    /// </summary>
    [HttpGet("{id:guid}/items")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<InspectionItemResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<InspectionItemResponse>>>> GetItems(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetItemsAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<InspectionItemResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Add an inspection item.
    /// </summary>
    [HttpPost("{id:guid}/items")]
    [ProducesResponseType(typeof(ApiResponse<InspectionItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<InspectionItemResponse>>> AddItem(
        Guid id,
        [FromBody] CreateInspectionItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.AddItemAsync(id, request, cancellationToken);
        return CreatedAtAction(nameof(GetItems), new { id }, ApiResponse<InspectionItemResponse>.SuccessResponse(result, "Inspection item added successfully."));
    }

    /// <summary>
    /// Update an inspection item.
    /// </summary>
    [HttpPut("{id:guid}/items/{itemId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<InspectionItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<InspectionItemResponse>>> UpdateItem(
        Guid id,
        Guid itemId,
        [FromBody] UpdateInspectionItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateItemAsync(id, itemId, request, cancellationToken);
        return Ok(ApiResponse<InspectionItemResponse>.SuccessResponse(result, "Inspection item updated successfully."));
    }
}
