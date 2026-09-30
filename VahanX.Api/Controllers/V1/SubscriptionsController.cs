using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Business;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for subscription operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _service;
    private readonly ILogger<SubscriptionsController> _logger;

    public SubscriptionsController(ISubscriptionService service, ILogger<SubscriptionsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Get all subscription plans.
    /// </summary>
    [HttpGet("plans")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SubscriptionPlanResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<SubscriptionPlanResponse>>>> GetPlans(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPlansAsync(page, pageSize, isActive, cancellationToken);
        return Ok(ApiResponse<PagedResult<SubscriptionPlanResponse>>.SuccessResponse(result));
    }

    /// <summary>
    /// Get subscription plan by ID.
    /// </summary>
    [HttpGet("plans/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionPlanResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SubscriptionPlanResponse>>> GetPlanById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetPlanByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Subscription plan not found."));

        return Ok(ApiResponse<SubscriptionPlanResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new subscription plan.
    /// </summary>
    [HttpPost("plans")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionPlanResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SubscriptionPlanResponse>>> CreatePlan(
        [FromBody] CreateSubscriptionPlanRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreatePlanAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetPlanById), new { id = result.Id }, ApiResponse<SubscriptionPlanResponse>.SuccessResponse(result, "Subscription plan created successfully."));
    }

    /// <summary>
    /// Update a subscription plan.
    /// </summary>
    [HttpPut("plans/{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionPlanResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SubscriptionPlanResponse>>> UpdatePlan(
        Guid id,
        [FromBody] UpdateSubscriptionPlanRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdatePlanAsync(id, request, cancellationToken);
        return Ok(ApiResponse<SubscriptionPlanResponse>.SuccessResponse(result, "Subscription plan updated successfully."));
    }

    /// <summary>
    /// Get my active subscription.
    /// </summary>
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<SubscriptionResponse>>> GetMySubscription(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetMySubscriptionAsync(userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("No active subscription found."));

        return Ok(ApiResponse<SubscriptionResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Get subscription by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SubscriptionResponse>>> GetSubscriptionById(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetSubscriptionByIdAsync(id, userId, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Subscription not found."));

        return Ok(ApiResponse<SubscriptionResponse>.SuccessResponse(result));
    }

    /// <summary>
    /// Create a new subscription.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<SubscriptionResponse>>> CreateSubscription(
        [FromBody] CreateSubscriptionRequest request,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateSubscriptionAsync(request, userId, cancellationToken);
        return CreatedAtAction(nameof(GetSubscriptionById), new { id = result.Id, userId }, ApiResponse<SubscriptionResponse>.SuccessResponse(result, "Subscription created successfully."));
    }

    /// <summary>
    /// Cancel a subscription.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SubscriptionResponse>>> CancelSubscription(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.CancelSubscriptionAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<SubscriptionResponse>.SuccessResponse(result, "Subscription cancelled successfully."));
    }

    /// <summary>
    /// Renew a subscription.
    /// </summary>
    [HttpPost("{id:guid}/renew")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SubscriptionResponse>>> RenewSubscription(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.RenewSubscriptionAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<SubscriptionResponse>.SuccessResponse(result, "Subscription renewed successfully."));
    }

    /// <summary>
    /// Change subscription plan.
    /// </summary>
    [HttpPost("{id:guid}/change-plan")]
    [ProducesResponseType(typeof(ApiResponse<SubscriptionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<SubscriptionResponse>>> ChangePlan(
        Guid id,
        [FromQuery] Guid userId,
        [FromBody] ChangePlanRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangePlanAsync(id, request, userId, cancellationToken);
        return Ok(ApiResponse<SubscriptionResponse>.SuccessResponse(result, "Subscription plan changed successfully."));
    }

    /// <summary>
    /// Get subscription usage.
    /// </summary>
    [HttpGet("{id:guid}/usage")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<SubscriptionUsageResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<PagedResult<SubscriptionUsageResponse>>>> GetSubscriptionUsage(
        Guid id,
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetSubscriptionUsageAsync(id, userId, cancellationToken);
        return Ok(ApiResponse<PagedResult<SubscriptionUsageResponse>>.SuccessResponse(result));
    }
}
