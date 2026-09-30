using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for verification operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VerificationsController : ControllerBase
{
    private readonly IVerificationService _service;
    private readonly ILogger<VerificationsController> _logger;

    public VerificationsController(IVerificationService service, ILogger<VerificationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all verifications with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<VerificationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<VerificationResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? vehicleId = null,
        [FromQuery] Guid? listingId = null,
        [FromQuery] VehicleVerificationStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, vehicleId, listingId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<VerificationResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get verification by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<VerificationResponse>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Verification not found."));

        return Ok(ApiResponse<VerificationResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get public verification information for a vehicle.
    /// </summary>
    [HttpGet("vehicle/{vehicleId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PublicVerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PublicVerificationResponse>>> GetPublicByVehicle(Guid vehicleId, CancellationToken cancellationToken)
    {
        var result = await _service.GetPublicByVehicleIdAsync(vehicleId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("No verified verification found for this vehicle."));

        return Ok(ApiResponse<PublicVerificationResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new verification.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VerificationResponse>>> Create(
        [FromBody] CreateVerificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<VerificationResponse>.SuccessResponse(result, "Verification created successfully."));
    }

    /// <summary>
    /// Update an existing verification.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VerificationResponse>>> Update(
        Guid id,
        [FromBody] UpdateVerificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return Ok(ApiResponse<VerificationResponse>.SuccessResponse(result, "Verification updated successfully."));
    }

    /// <summary>
    /// Delete a verification.
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
    /// Submit a verification for review.
    /// </summary>
    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VerificationResponse>>> Submit(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.SubmitAsync(id, cancellationToken);
        return Ok(ApiResponse<VerificationResponse>.SuccessResponse(result, "Verification submitted for review."));
    }

    /// <summary>
    /// Approve a verification.
    /// </summary>
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VerificationResponse>>> Approve(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.ApproveAsync(id, cancellationToken);
        return Ok(ApiResponse<VerificationResponse>.SuccessResponse(result, "Verification approved successfully."));
    }

    /// <summary>
    /// Reject a verification.
    /// </summary>
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VerificationResponse>>> Reject(
        Guid id,
        [FromBody] RejectVerificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.RejectAsync(id, request.Reason, cancellationToken);
        return Ok(ApiResponse<VerificationResponse>.SuccessResponse(result, "Verification rejected successfully."));
    }

    /// <summary>
    /// Revoke a verification.
    /// </summary>
    [HttpPost("{id:guid}/revoke")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<VerificationResponse>>> Revoke(
        Guid id,
        [FromBody] RevokeVerificationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.RevokeAsync(id, request.Reason, cancellationToken);
        return Ok(ApiResponse<VerificationResponse>.SuccessResponse(result, "Verification revoked successfully."));
    }
}

/// <summary>
/// Request to reject a verification.
/// </summary>
public class RejectVerificationRequest
{
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// Request to revoke a verification.
/// </summary>
public class RevokeVerificationRequest
{
    public string Reason { get; set; } = string.Empty;
}
