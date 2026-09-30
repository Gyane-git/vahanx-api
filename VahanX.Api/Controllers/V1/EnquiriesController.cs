using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Engagement;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for enquiry operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class EnquiriesController : ControllerBase
{
    private readonly IEnquiryService _service;
    private readonly ILogger<EnquiriesController> _logger;

    public EnquiriesController(IEnquiryService service, ILogger<EnquiriesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all enquiries with pagination and filters.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EnquiryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<EnquiryResponse>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] Guid? listingId = null,
        [FromQuery] Guid? sellerId = null,
        [FromQuery] Guid? dealerId = null,
        [FromQuery] EnquiryStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, userId, listingId, sellerId, dealerId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<EnquiryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get enquiry by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EnquiryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<EnquiryResponse>>> GetById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Enquiry not found."));

        return Ok(ApiResponse<EnquiryResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new enquiry.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EnquiryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<EnquiryResponse>>> Create(
        [FromBody] CreateEnquiryRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id, userId }, ApiResponse<EnquiryResponse>.SuccessResponse(result, "Enquiry created successfully."));
    }

    /// <summary>
    /// Update an existing enquiry.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<EnquiryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<EnquiryResponse>>> Update(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateEnquiryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<EnquiryResponse>.SuccessResponse(result, "Enquiry updated successfully."));
    }

    /// <summary>
    /// Delete an enquiry.
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
    /// Respond to an enquiry.
    /// </summary>
    [HttpPost("{id:guid}/respond")]
    [ProducesResponseType(typeof(ApiResponse<EnquiryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<EnquiryResponse>>> Respond(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] RespondEnquiryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.RespondAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<EnquiryResponse>.SuccessResponse(result, "Enquiry responded successfully."));
    }

    /// <summary>
    /// Close an enquiry.
    /// </summary>
    [HttpPost("{id:guid}/close")]
    [ProducesResponseType(typeof(ApiResponse<EnquiryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<EnquiryResponse>>> Close(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CloseAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<EnquiryResponse>.SuccessResponse(result, "Enquiry closed successfully."));
    }

    /// <summary>
    /// Cancel an enquiry.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<EnquiryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<EnquiryResponse>>> Cancel(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CancelAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<EnquiryResponse>.SuccessResponse(result, "Enquiry cancelled successfully."));
    }

    /// <summary>
    /// Get enquiries for a specific listing.
    /// </summary>
    [HttpGet("listing/{listingId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EnquiryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<EnquiryResponse>>>> GetByListing(
        Guid listingId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByListingAsync(listingId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<EnquiryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get enquiries for a specific seller.
    /// </summary>
    [HttpGet("seller/{sellerId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EnquiryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<EnquiryResponse>>>> GetBySeller(
        Guid sellerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetBySellerAsync(sellerId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<EnquiryResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get enquiries for a specific dealer.
    /// </summary>
    [HttpGet("dealer/{dealerId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EnquiryResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<EnquiryResponse>>>> GetByDealer(
        Guid dealerId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByDealerAsync(dealerId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<EnquiryResponse>>.SuccessResponse(result));
    }
}
