using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for service center operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ServiceCentersController : ControllerBase
{
    private readonly IServiceCenterService _service;
    private readonly ILogger<ServiceCentersController> _logger;

    public ServiceCentersController(IServiceCenterService service, ILogger<ServiceCentersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all service centers with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceCenterListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceCenterListDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] Guid? locationId = null,
        [FromQuery] ServiceCenterStatus? status = null,
        [FromQuery] VerificationStatus? verificationStatus = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, keyword, locationId, status, verificationStatus, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceCenterListDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get service center by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceCenterDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ServiceCenterDetailsDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Service center not found."));

        return Ok(ApiResponse<ServiceCenterDetailsDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Get nearby service centers.
    /// </summary>
    [HttpGet("nearby")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceCenterListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceCenterListDto>>>> GetNearby(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm = 10,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetNearbyAsync(latitude, longitude, radiusKm, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceCenterListDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new service center.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ServiceCenterDetailsDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceCenterDetailsDto>>> Create(
        [FromBody] CreateServiceCenterRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ServiceCenterDetailsDto>.SuccessResponse(result, "Service center created successfully."));
    }

    /// <summary>
    /// Update a service center.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceCenterDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceCenterDetailsDto>>> Update(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateServiceCenterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<ServiceCenterDetailsDto>.SuccessResponse(result, "Service center updated successfully."));
    }

    /// <summary>
    /// Delete a service center.
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
    /// Get branches for a service center.
    /// </summary>
    [HttpGet("{id:guid}/branches")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceCenterBranchResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceCenterBranchResponse>>>> GetBranches(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBranchesAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceCenterBranchResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get a specific branch.
    /// </summary>
    [HttpGet("branches/{branchId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceCenterBranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ServiceCenterBranchResponse>>> GetBranchById(
        Guid id,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetBranchByIdAsync(id, branchId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Branch not found."));

        return Ok(ApiResponse<ServiceCenterBranchResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a branch for a service center.
    /// </summary>
    [HttpPost("{id:guid}/branches")]
    [ProducesResponseType(typeof(ApiResponse<ServiceCenterBranchResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceCenterBranchResponse>>> CreateBranch(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] CreateServiceCenterBranchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateBranchAsync(id, request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetBranches), new { id }, ApiResponse<ServiceCenterBranchResponse>.SuccessResponse(result, "Branch created successfully."));
    }

    /// <summary>
    /// Update a branch.
    /// </summary>
    [HttpPut("branches/{branchId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceCenterBranchResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<ServiceCenterBranchResponse>>> UpdateBranch(
        Guid id,
        Guid branchId,
        [FromQuery] Guid userId,
        [FromBody] UpdateServiceCenterBranchRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateBranchAsync(id, branchId, request, userId, cancellationToken);
        return Ok(ApiResponse<ServiceCenterBranchResponse>.SuccessResponse(result, "Branch updated successfully."));
    }

    /// <summary>
    /// Delete a branch.
    /// </summary>
    [HttpDelete("branches/{branchId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteBranch(
        Guid id,
        Guid branchId,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        await _service.DeleteBranchAsync(id, branchId, userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Get services offered by a branch.
    /// </summary>
    [HttpGet("branches/{branchId:guid}/services")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceCenterServiceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceCenterServiceResponse>>>> GetBranchServices(
        Guid id,
        Guid branchId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBranchServicesAsync(branchId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceCenterServiceResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get packages offered by a branch.
    /// </summary>
    [HttpGet("branches/{branchId:guid}/packages")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServicePackageResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServicePackageResponse>>>> GetBranchPackages(
        Guid id,
        Guid branchId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBranchPackagesAsync(branchId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServicePackageResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get working hours for a branch.
    /// </summary>
    [HttpGet("branches/{branchId:guid}/working-hours")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceWorkingHourResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceWorkingHourResponse>>>> GetBranchWorkingHours(
        Guid id,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetBranchWorkingHoursAsync(branchId, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceWorkingHourResponse>>.SuccessResponse(result));
    }
}
