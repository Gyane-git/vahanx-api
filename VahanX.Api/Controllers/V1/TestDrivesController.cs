using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for test drive operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TestDrivesController : ControllerBase
{
    private readonly ITestDriveService _service;
    private readonly ILogger<TestDrivesController> _logger;

    public TestDrivesController(ITestDriveService service, ILogger<TestDrivesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all test drives with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDriveResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<TestDriveResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] Guid? listingId = null,
        [FromQuery] Guid? dealerId = null,
        [FromQuery] TestDriveStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, userId, listingId, dealerId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<TestDriveResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get test drive by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TestDriveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<TestDriveResponse>>> GetById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Test drive not found."));

        return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new test drive request.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TestDriveResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<TestDriveResponse>>> Create(
        [FromBody] CreateTestDriveRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, userId }, ApiResponse<TestDriveResponse>.SuccessResponse(result, "Test drive request created successfully."));
    }

    /// <summary>
    /// Update a test drive.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TestDriveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<TestDriveResponse>>> Update(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateTestDriveRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(result, "Test drive updated successfully."));
    }

    /// <summary>
    /// Delete a test drive.
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
    /// Confirm a test drive.
    /// </summary>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<TestDriveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<TestDriveResponse>>> Confirm(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.ConfirmAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(result, "Test drive confirmed successfully."));
    }

    /// <summary>
    /// Reschedule a test drive.
    /// </summary>
    [HttpPost("{id:guid}/reschedule")]
    [ProducesResponseType(typeof(ApiResponse<TestDriveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<TestDriveResponse>>> Reschedule(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] RescheduleTestDriveRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.RescheduleAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(result, "Test drive rescheduled successfully."));
    }

    /// <summary>
    /// Cancel a test drive.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<TestDriveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<TestDriveResponse>>> Cancel(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CancelAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(result, "Test drive cancelled successfully."));
    }

    /// <summary>
    /// Complete a test drive.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<TestDriveResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<TestDriveResponse>>> Complete(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CompleteAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<TestDriveResponse>.SuccessResponse(result, "Test drive completed successfully."));
    }

    /// <summary>
    /// Get test drives for a specific listing.
    /// </summary>
    [HttpGet("listing/{listingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDriveResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<TestDriveResponse>>>> GetByListing(
        Guid listingId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByListingAsync(listingId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<TestDriveResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get test drives for a specific dealer.
    /// </summary>
    [HttpGet("dealer/{dealerId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<TestDriveResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<TestDriveResponse>>>> GetByDealer(
        Guid dealerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByDealerAsync(dealerId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<TestDriveResponse>>.SuccessResponse(result));
    }
}
