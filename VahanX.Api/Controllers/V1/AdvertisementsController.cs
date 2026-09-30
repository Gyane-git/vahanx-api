using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for advertisement operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AdvertisementsController : ControllerBase
{
    private readonly IAdvertisementService _service;
    private readonly ILogger<AdvertisementsController> _logger;

    public AdvertisementsController(IAdvertisementService service, ILogger<AdvertisementsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all advertisement campaigns.
    /// </summary>
    [HttpGet("campaigns")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AdvertisementCampaignResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<AdvertisementCampaignResponse>>>> GetCampaigns(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Guid? userId = null,
        [FromQuery] Domain.Enums.AdvertisementCampaignStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCampaignsAsync(page, pageSize, userId, status, cancellationToken);
        return Ok(ApiResponse<PagedResult<AdvertisementCampaignResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get campaign by ID.
    /// </summary>
    [HttpGet("campaigns/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementCampaignResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdvertisementCampaignResponse>>> GetCampaignById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetCampaignByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Campaign not found."));

        return Ok(ApiResponse<AdvertisementCampaignResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new advertisement campaign.
    /// </summary>
    [HttpPost("campaigns")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementCampaignResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AdvertisementCampaignResponse>>> CreateCampaign(
        [FromBody] CreateAdvertisementCampaignRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateCampaignAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetCampaignById), new { id = result.Id, userId }, ApiResponse<AdvertisementCampaignResponse>.SuccessResponse(result, "Campaign created successfully."));
    }

    /// <summary>
    /// Update a campaign.
    /// </summary>
    [HttpPut("campaigns/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementCampaignResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdvertisementCampaignResponse>>> UpdateCampaign(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateAdvertisementCampaignRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateCampaignAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<AdvertisementCampaignResponse>.SuccessResponse(result, "Campaign updated successfully."));
    }

    /// <summary>
    /// Submit a campaign for review.
    /// </summary>
    [HttpPost("campaigns/{id:guid}/submit")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementCampaignResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdvertisementCampaignResponse>>> SubmitCampaign(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.SubmitCampaignAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<AdvertisementCampaignResponse>.SuccessResponse(result, "Campaign submitted for review."));
    }

    /// <summary>
    /// Pause a campaign.
    /// </summary>
    [HttpPost("campaigns/{id:guid}/pause")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementCampaignResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdvertisementCampaignResponse>>> PauseCampaign(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.PauseCampaignAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<AdvertisementCampaignResponse>.SuccessResponse(result, "Campaign paused successfully."));
    }

    /// <summary>
    /// Resume a campaign.
    /// </summary>
    [HttpPost("campaigns/{id:guid}/resume")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementCampaignResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdvertisementCampaignResponse>>> ResumeCampaign(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.ResumeCampaignAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<AdvertisementCampaignResponse>.SuccessResponse(result, "Campaign resumed successfully."));
    }

    /// <summary>
    /// Cancel a campaign.
    /// </summary>
    [HttpPost("campaigns/{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementCampaignResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdvertisementCampaignResponse>>> CancelCampaign(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CancelCampaignAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<AdvertisementCampaignResponse>.SuccessResponse(result, "Campaign cancelled successfully."));
    }

    /// <summary>
    /// Create a new advertisement.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AdvertisementResponse>>> CreateAdvertisement(
        [FromBody] CreateAdvertisementRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAdvertisementAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetAdvertisementById), new { id = result.Id }, ApiResponse<AdvertisementResponse>.SuccessResponse(result, "Advertisement created successfully."));
    }

    /// <summary>
    /// Get advertisement by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AdvertisementResponse>>> GetAdvertisementById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetAdvertisementByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Advertisement not found."));

        return Ok(ApiResponse<AdvertisementResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Update an advertisement.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AdvertisementResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<AdvertisementResponse>>> UpdateAdvertisement(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] UpdateAdvertisementRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAdvertisementAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<AdvertisementResponse>.SuccessResponse(result, "Advertisement updated successfully."));
    }

    /// <summary>
    /// Get advertisements for a campaign.
    /// </summary>
    [HttpGet("campaigns/{campaignId:guid}/advertisements")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AdvertisementResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<AdvertisementResponse>>>> GetCampaignAdvertisements(
        Guid campaignId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetCampaignAdvertisementsAsync(campaignId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<AdvertisementResponse>>.SuccessResponse(result));
    }
}
