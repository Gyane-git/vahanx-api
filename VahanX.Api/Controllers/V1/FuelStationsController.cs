using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Services;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for fuel station operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class FuelStationsController : ControllerBase
{
    private readonly IFuelStationService _service;
    private readonly ILogger<FuelStationsController> _logger;

    public FuelStationsController(IFuelStationService service, ILogger<FuelStationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all fuel stations with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelStationListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelStationListDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] Guid? locationId = null,
        [FromQuery] FuelStationStatus? status = null,
        [FromQuery] VerificationStatus? verificationStatus = null,
        [FromQuery] bool? is24Hours = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, keyword, locationId, status, verificationStatus, is24Hours, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelStationListDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get fuel station by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FuelStationDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<FuelStationDetailsDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Fuel station not found."));

        return Ok(ApiResponse<FuelStationDetailsDto>.SuccessResponse(result));
    }

    /// <summary>
    /// Get nearby fuel stations.
    /// </summary>
    [HttpGet("nearby")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelStationListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelStationListDto>>>> GetNearby(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] double radiusKm = 10,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetNearbyAsync(latitude, longitude, radiusKm, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelStationListDto>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get fuel types for a fuel station.
    /// </summary>
    [HttpGet("{id:guid}/fuel-types")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelStationFuelTypeResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelStationFuelTypeResponse>>>> GetFuelTypes(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetFuelTypesAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelStationFuelTypeResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get availability for a fuel station.
    /// </summary>
    [HttpGet("{id:guid}/availability")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelStationAvailabilityResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelStationAvailabilityResponse>>>> GetAvailability(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAvailabilityAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelStationAvailabilityResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get prices for a fuel station.
    /// </summary>
    [HttpGet("{id:guid}/prices")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelStationPriceResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelStationPriceResponse>>>> GetPrices(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPricesAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelStationPriceResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get amenities for a fuel station.
    /// </summary>
    [HttpGet("{id:guid}/amenities")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelStationAmenityResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelStationAmenityResponse>>>> GetAmenities(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAmenitiesAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelStationAmenityResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get working hours for a fuel station.
    /// </summary>
    [HttpGet("{id:guid}/working-hours")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FuelStationWorkingHourResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<FuelStationWorkingHourResponse>>>> GetWorkingHours(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetWorkingHoursAsync(id, cancellationToken);
        return Ok(ApiResponse<PagedResult<FuelStationWorkingHourResponse>>.SuccessResponse(result));
    }
}
