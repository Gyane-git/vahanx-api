using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for sell vehicle operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SellVehiclesController : ControllerBase
{
    private readonly ISellVehicleService _service;
    private readonly ILogger<SellVehiclesController> _logger;

    public SellVehiclesController(ISellVehicleService service, ILogger<SellVehiclesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all sell requests.
    /// </summary>
    [HttpGet("requests")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SellRequestResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<SellRequestResponse>>>> GetRequests(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] Domain.Enums.SellRequestStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetRequestsAsync(page, pageSize, userId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<SellRequestResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get sell request by ID.
    /// </summary>
    [HttpGet("requests/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SellRequestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellRequestResponse>>> GetRequestById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetRequestByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Sell request not found."));

        return Ok(ApiResponse<SellRequestResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new sell request.
    /// </summary>
    [HttpPost("requests")]
    [ProducesResponseType(typeof(ApiResponse<SellRequestResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SellRequestResponse>>> CreateRequest(
        [FromBody] CreateSellRequestRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateRequestAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetRequestById), new { id = result.Id, userId }, ApiResponse<SellRequestResponse>.SuccessResponse(result, "Sell request created successfully."));
    }

    /// <summary>
    /// Update a sell request.
    /// </summary>
    [HttpPut("requests/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SellRequestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellRequestResponse>>> UpdateRequest(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateSellRequestRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateRequestAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<SellRequestResponse>.SuccessResponse(result, "Sell request updated successfully."));
    }

    /// <summary>
    /// Submit a sell request.
    /// </summary>
    [HttpPost("requests/{id:guid}/submit")]
    [ProducesResponseType(typeof(ApiResponse<SellRequestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellRequestResponse>>> SubmitRequest(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.SubmitRequestAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<SellRequestResponse>.SuccessResponse(result, "Sell request submitted successfully."));
    }

    /// <summary>
    /// Cancel a sell request.
    /// </summary>
    [HttpPost("requests/{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<SellRequestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellRequestResponse>>> CancelRequest(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CancelRequestAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<SellRequestResponse>.SuccessResponse(result, "Sell request cancelled successfully."));
    }

    /// <summary>
    /// Create a sell vehicle for a request.
    /// </summary>
    [HttpPost("requests/{id:guid}/vehicle")]
    [ProducesResponseType(typeof(ApiResponse<SellVehicleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellVehicleResponse>>> CreateSellVehicle(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] CreateSellVehicleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateSellVehicleAsync(id, request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetSellVehicle), new { requestId = id, userId }, ApiResponse<SellVehicleResponse>.SuccessResponse(result, "Sell vehicle created successfully."));
    }

    /// <summary>
    /// Get sell vehicle for a request.
    /// </summary>
    [HttpGet("requests/{id:guid}/vehicle")]
    [ProducesResponseType(typeof(ApiResponse<SellVehicleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellVehicleResponse>>> GetSellVehicle(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetSellVehicleAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Sell vehicle not found."));

        return Ok(ApiResponse<SellVehicleResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get offers for a sell request.
    /// </summary>
    [HttpGet("requests/{id:guid}/offers")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SellOfferResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PagedResult<SellOfferResponse>>>> GetRequestOffers(
        Guid id,
        [FromQuery] Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetRequestOffersAsync(id, userId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<SellOfferResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Create an offer on a sell request.
    /// </summary>
    [HttpPost("requests/{id:guid}/offers")]
    [ProducesResponseType(typeof(ApiResponse<SellOfferResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<SellOfferResponse>>> CreateOffer(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] CreateSellOfferRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateOfferAsync(id, request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetRequestOffers), new { id, userId }, ApiResponse<SellOfferResponse>.SuccessResponse(result, "Offer created successfully."));
    }

    /// <summary>
    /// Accept an offer.
    /// </summary>
    [HttpPost("offers/{offerId:guid}/accept")]
    [ProducesResponseType(typeof(ApiResponse<SellOfferResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellOfferResponse>>> AcceptOffer(
        Guid offerId,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.AcceptOfferAsync(offerId, userId, cancellationToken);
        return Ok(ApiResponse<SellOfferResponse>.SuccessResponse(result, "Offer accepted successfully."));
    }

    /// <summary>
    /// Reject an offer.
    /// </summary>
    [HttpPost("offers/{offerId:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<SellOfferResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SellOfferResponse>>> RejectOffer(
        Guid offerId,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.RejectOfferAsync(offerId, userId, cancellationToken);
        return Ok(ApiResponse<SellOfferResponse>.SuccessResponse(result, "Offer rejected successfully."));
    }
}
