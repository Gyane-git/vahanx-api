using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for service booking operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ServiceBookingsController : ControllerBase
{
    private readonly IServiceBookingService _service;
    private readonly ILogger<ServiceBookingsController> _logger;

    public ServiceBookingsController(IServiceBookingService service, ILogger<ServiceBookingsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all service bookings with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceBookingResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceBookingResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] Guid? branchId = null,
        [FromQuery] ServiceBookingStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, userId, branchId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceBookingResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get service booking by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> GetById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Booking not found."));

        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get my bookings.
    /// </summary>
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceBookingResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceBookingResponse>>>> GetMyBookings(
        [FromQuery] Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetMyBookingsAsync(userId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceBookingResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new service booking.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Create(
        [FromBody] CreateServiceBookingRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, userId }, ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking created successfully."));
    }

    /// <summary>
    /// Update a service booking.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Update(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateServiceBookingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking updated successfully."));
    }

    /// <summary>
    /// Delete a service booking.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Confirm a booking.
    /// </summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Confirm(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.ConfirmAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking confirmed successfully."));
    }

    /// <summary>
    /// Reschedule a booking.
    /// </summary>
    [HttpPost("{id:guid}/reschedule")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Reschedule(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] RescheduleServiceBookingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.RescheduleAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking rescheduled successfully."));
    }

    /// <summary>
    /// Start a booking.
    /// </summary>
    [HttpPost("{id:guid}/start")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Start(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.StartAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking started successfully."));
    }

    /// <summary>
    /// Complete a booking.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Complete(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CompleteAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking completed successfully."));
    }

    /// <summary>
    /// Cancel a booking.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Cancel(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CancelAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking cancelled successfully."));
    }

    /// <summary>
    /// Reject a booking.
    /// </summary>
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> Reject(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.RejectAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking rejected successfully."));
    }

    /// <summary>
    /// Mark booking as no-show.
    /// </summary>
    [HttpPost("{id:guid}/no-show")]
    [ProducesResponseType(typeof(ApiResponse<ServiceBookingResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceBookingResponse>>> NoShow(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.NoShowAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<ServiceBookingResponse>.SuccessResponse(result, "Booking marked as no-show."));
    }

    /// <summary>
    /// Get bookings for a specific branch.
    /// </summary>
    [HttpGet("branch/{branchId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceBookingResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceBookingResponse>>>> GetBranchBookings(
        Guid branchId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBranchBookingsAsync(branchId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceBookingResponse>>.SuccessResponse(result));
    }
}
