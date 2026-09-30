using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for EV charging station operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ChargingStationsController : ControllerBase
{
    private readonly IChargingStationService _service;
    private readonly ILogger<ChargingStationsController> _logger;

    public ChargingStationsController(IChargingStationService service, ILogger<ChargingStationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all charging stations with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ChargingStationListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ChargingStationListDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] Guid? locationId = null,
        [FromQuery] ChargingStationStatus? status = null,
        [FromQuery] VerificationStatus? verificationStatus = null,
        [FromQuery] bool? is24Hours = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, keyword, locationId, status, verificationStatus, is24Hours, cancellationToken);
        return Ok(ApiResponse<PagedResult<ChargingStationListDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get charging station by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ChargingStationDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ChargingStationDetailsDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Charging station not found."));

        return Ok(ApiResponse<ChargingStationDetailsDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Get nearby charging stations.
    /// </summary>
    [HttpGet("nearby")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ChargingStationListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ChargingStationListDto>>>> GetNearby(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm = 10,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetNearbyAsync(latitude, longitude, radiusKm, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ChargingStationListDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get connectors for a charging station.
    /// </summary>
    [HttpGet("{id:guid}/connectors")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ChargingStationConnectorResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ChargingStationConnectorResponse>>>> GetConnectors(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetConnectorsAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ChargingStationConnectorResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get amenities for a charging station.
    /// </summary>
    [HttpGet("{id:guid}/amenities")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ChargingStationAmenityResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ChargingStationAmenityResponse>>>> GetAmenities(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAmenitiesAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ChargingStationAmenityResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get availability for a charging station.
    /// </summary>
    [HttpGet("{id:guid}/availability")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ChargingStationAvailabilityResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ChargingStationAvailabilityResponse>>>> GetAvailability(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAvailabilityAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ChargingStationAvailabilityResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get prices for a charging station.
    /// </summary>
    [HttpGet("{id:guid}/prices")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ChargingStationPriceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ChargingStationPriceResponse>>>> GetPrices(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPricesAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ChargingStationPriceResponse>>.SuccessResponse(result));
    }
}
