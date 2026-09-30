using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Trust;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for vehicle history operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/vehicles/{vehicleId:guid}/history")]
[ApiVersion("1.0")]
public class VehicleHistoryController : ControllerBase
{
    private readonly IVehicleHistoryService _service;
    private readonly ILogger<VehicleHistoryController> _logger;

    public VehicleHistoryController(IVehicleHistoryService service, ILogger<VehicleHistoryController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get aggregated vehicle history.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<VehicleHistoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<VehicleHistoryResponse>>> GetVehicleHistory(
        Guid vehicleId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetVehicleHistoryAsync(vehicleId, cancellationToken);
        return Ok(ApiResponse<VehicleHistoryResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get ownership history for a vehicle.
    /// </summary>
    [HttpGet("ownership")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<OwnershipHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<OwnershipHistoryResponse>>>> GetOwnershipHistory(
        Guid vehicleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetOwnershipHistoryAsync(vehicleId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<OwnershipHistoryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get mileage history for a vehicle.
    /// </summary>
    [HttpGet("mileage")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MileageHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<MileageHistoryResponse>>>> GetMileageHistory(
        Guid vehicleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetMileageHistoryAsync(vehicleId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<MileageHistoryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get service history for a vehicle.
    /// </summary>
    [HttpGet("service")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ServiceHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ServiceHistoryResponse>>>> GetServiceHistory(
        Guid vehicleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetServiceHistoryAsync(vehicleId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ServiceHistoryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get accident history for a vehicle.
    /// </summary>
    [HttpGet("accident")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AccidentHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<AccidentHistoryResponse>>>> GetAccidentHistory(
        Guid vehicleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAccidentHistoryAsync(vehicleId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<AccidentHistoryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get insurance history for a vehicle.
    /// </summary>
    [HttpGet("insurance")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<InsuranceHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<InsuranceHistoryResponse>>>> GetInsuranceHistory(
        Guid vehicleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetInsuranceHistoryAsync(vehicleId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<InsuranceHistoryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get registration history for a vehicle.
    /// </summary>
    [HttpGet("registration")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<RegistrationHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<RegistrationHistoryResponse>>>> GetRegistrationHistory(
        Guid vehicleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetRegistrationHistoryAsync(vehicleId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<RegistrationHistoryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get price history for a listing.
    /// </summary>
    [HttpGet("price/{listingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PriceHistoryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<PriceHistoryResponse>>>> GetPriceHistory(
        Guid vehicleId,
        Guid listingId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPriceHistoryAsync(listingId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<PriceHistoryResponse>>.SuccessResponse(result));
    }
}
