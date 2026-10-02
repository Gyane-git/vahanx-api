using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Moderation;
using VahanX.Domain.Enums;
using VahanX.Application.DTOs.Admin;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for moderation operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/admin/moderation")]
[ApiVersion("1.0")]
public class ModerationController : ControllerBase
{
    private readonly IModerationService _moderationService;
    private readonly ILogger<ModerationController> _logger;

    public ModerationController(IModerationService moderationService, ILogger<ModerationController> logger)
    {
        _moderationService = moderationService;
        _logger = logger;
    }

    // Reports

    [HttpGet("reports")]
    [Authorize(Policy = AdminPermissions.ReportView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ReportResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ReportResponse>>>> GetReports(
        [FromQuery] ModerationReportStatus? status,
        [FromQuery] ReportPriority? priority,
        [FromQuery] Guid? assignedToUserId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _moderationService.GetReportsAsync(status, priority, assignedToUserId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ReportResponse>>.SuccessResponse(result));
    }

    [HttpGet("reports/{id:guid}")]
    [Authorize(Policy = AdminPermissions.ReportView)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> GetReportById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _moderationService.GetReportByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Report not found."));
        return Ok(ApiResponse<ReportResponse>.SuccessResponse(result));
    }

    [HttpPost("reports")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> CreateReport(
        [FromBody] CreateReportRequest request,
        [FromQuery] Guid reporterUserId,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.CreateReportAsync(request, reporterUserId, cancellationToken);
        return CreatedAtAction(nameof(GetReportById), new { id = result.Id }, ApiResponse<ReportResponse>.SuccessResponse(result, "Report created successfully."));
    }

    [HttpPost("reports/{id:guid}/assign")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> AssignReport(
        Guid id,
        [FromBody] AssignReportRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.AssignReportAsync(id, request, cancellationToken);
        return Ok(ApiResponse<ReportResponse>.SuccessResponse(result, "Report assigned successfully."));
    }

    [HttpPost("reports/{id:guid}/resolve")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> ResolveReport(
        Guid id,
        [FromBody] ResolveReportRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.ResolveReportAsync(id, request, cancellationToken);
        return Ok(ApiResponse<ReportResponse>.SuccessResponse(result, "Report resolved successfully."));
    }

    [HttpPost("reports/{id:guid}/reject")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> RejectReport(
        Guid id,
        [FromQuery] string reason,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.RejectReportAsync(id, reason, cancellationToken);
        return Ok(ApiResponse<ReportResponse>.SuccessResponse(result, "Report rejected successfully."));
    }

    [HttpPost("reports/{id:guid}/escalate")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> EscalateReport(Guid id, CancellationToken cancellationToken)
    {
        var result = await _moderationService.EscalateReportAsync(id, cancellationToken);
        return Ok(ApiResponse<ReportResponse>.SuccessResponse(result, "Report escalated successfully."));
    }

    // Cases

    [HttpGet("cases")]
    [Authorize(Policy = AdminPermissions.ReportView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ModerationCaseResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ModerationCaseResponse>>>> GetCases(
        [FromQuery] ModerationCaseStatus? status,
        [FromQuery] ReportPriority? priority,
        [FromQuery] Guid? assignedToUserId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _moderationService.GetCasesAsync(status, priority, assignedToUserId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ModerationCaseResponse>>.SuccessResponse(result));
    }

    [HttpGet("cases/{id:guid}")]
    [Authorize(Policy = AdminPermissions.ReportView)]
    [ProducesResponseType(typeof(ApiResponse<ModerationCaseResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ModerationCaseResponse>>> GetCaseById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _moderationService.GetCaseByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Moderation case not found."));
        return Ok(ApiResponse<ModerationCaseResponse>.SuccessResponse(result));
    }

    [HttpPost("cases")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ModerationCaseResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<ModerationCaseResponse>>> CreateCase(
        [FromBody] CreateModerationCaseRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.CreateCaseAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetCaseById), new { id = result.Id }, ApiResponse<ModerationCaseResponse>.SuccessResponse(result, "Moderation case created successfully."));
    }

    [HttpPost("cases/{id:guid}/assign")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ModerationCaseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ModerationCaseResponse>>> AssignCase(
        Guid id,
        [FromQuery] Guid assignedToUserId,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.AssignCaseAsync(id, assignedToUserId, cancellationToken);
        return Ok(ApiResponse<ModerationCaseResponse>.SuccessResponse(result, "Case assigned successfully."));
    }

    [HttpPost("cases/{id:guid}/action")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ModerationCaseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ModerationCaseResponse>>> PerformAction(
        Guid id,
        [FromBody] ModerationActionRequest request,
        [FromQuery] Guid performedByUserId,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.PerformActionAsync(id, request, performedByUserId, cancellationToken);
        return Ok(ApiResponse<ModerationCaseResponse>.SuccessResponse(result, "Action performed successfully."));
    }

    [HttpPost("cases/{id:guid}/resolve")]
    [Authorize(Policy = AdminPermissions.ReportManage)]
    [ProducesResponseType(typeof(ApiResponse<ModerationCaseResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ModerationCaseResponse>>> ResolveCase(
        Guid id,
        [FromBody] ResolveModerationCaseRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.ResolveCaseAsync(id, request, cancellationToken);
        return Ok(ApiResponse<ModerationCaseResponse>.SuccessResponse(result, "Case resolved successfully."));
    }

    // Listing Moderation

    [HttpGet("listings/pending")]
    [Authorize(Policy = AdminPermissions.ReportView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ListingModerationResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<ListingModerationResponse>>>> GetPendingListings(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _moderationService.GetPendingListingsAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ListingModerationResponse>>.SuccessResponse(result));
    }

    [HttpPost("listings/{id:guid}/approve")]
    [Authorize(Policy = AdminPermissions.ListingModerate)]
    [ProducesResponseType(typeof(ApiResponse<ListingModerationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingModerationResponse>>> ApproveListing(
        Guid id,
        [FromQuery] Guid moderatorUserId,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.ApproveListingAsync(id, moderatorUserId, cancellationToken);
        return Ok(ApiResponse<ListingModerationResponse>.SuccessResponse(result, "Listing approved successfully."));
    }

    [HttpPost("listings/{id:guid}/reject")]
    [Authorize(Policy = AdminPermissions.ListingReject)]
    [ProducesResponseType(typeof(ApiResponse<ListingModerationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingModerationResponse>>> RejectListing(
        Guid id,
        [FromQuery] Guid moderatorUserId,
        [FromQuery] string reason,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.RejectListingAsync(id, moderatorUserId, reason, cancellationToken);
        return Ok(ApiResponse<ListingModerationResponse>.SuccessResponse(result, "Listing rejected successfully."));
    }

    [HttpPost("listings/{id:guid}/hide")]
    [Authorize(Policy = AdminPermissions.ListingModerate)]
    [ProducesResponseType(typeof(ApiResponse<ListingModerationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingModerationResponse>>> HideListing(
        Guid id,
        [FromQuery] Guid moderatorUserId,
        [FromQuery] string reason,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.HideListingAsync(id, moderatorUserId, reason, cancellationToken);
        return Ok(ApiResponse<ListingModerationResponse>.SuccessResponse(result, "Listing hidden successfully."));
    }

    [HttpPost("listings/{id:guid}/restore")]
    [Authorize(Policy = AdminPermissions.ListingModerate)]
    [ProducesResponseType(typeof(ApiResponse<ListingModerationResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<ListingModerationResponse>>> RestoreListing(
        Guid id,
        [FromQuery] Guid moderatorUserId,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.RestoreListingAsync(id, moderatorUserId, cancellationToken);
        return Ok(ApiResponse<ListingModerationResponse>.SuccessResponse(result, "Listing restored successfully."));
    }

    // User Restrictions

    [HttpPost("users/{id:guid}/restrict")]
    [Authorize(Policy = AdminPermissions.UserSuspend)]
    [ProducesResponseType(typeof(ApiResponse<UserRestrictionResponse>), StatusCodes.Status201Created)]
    public async Task<ActionResult<ApiResponse<UserRestrictionResponse>>> CreateUserRestriction(
        Guid id,
        [FromBody] CreateUserRestrictionRequest request,
        [FromQuery] Guid createdByUserId,
        CancellationToken cancellationToken)
    {
        request.UserId = id;
        var result = await _moderationService.CreateUserRestrictionAsync(request, createdByUserId, cancellationToken);
        return CreatedAtAction(nameof(GetUserRestrictions), new { userId = id }, ApiResponse<UserRestrictionResponse>.SuccessResponse(result, "User restriction created successfully."));
    }

    [HttpGet("users/{id:guid}/restrictions")]
    [Authorize(Policy = AdminPermissions.ReportView)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<UserRestrictionResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<UserRestrictionResponse>>>> GetUserRestrictions(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _moderationService.GetUserRestrictionsAsync(id, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<UserRestrictionResponse>>.SuccessResponse(result));
    }

    [HttpPost("users/restrictions/{id:guid}/deactivate")]
    [Authorize(Policy = AdminPermissions.UserSuspend)]
    [ProducesResponseType(typeof(ApiResponse<UserRestrictionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<UserRestrictionResponse>>> DeactivateUserRestriction(Guid id, CancellationToken cancellationToken)
    {
        var result = await _moderationService.DeactivateUserRestrictionAsync(id, cancellationToken);
        return Ok(ApiResponse<UserRestrictionResponse>.SuccessResponse(result, "User restriction deactivated successfully."));
    }

    // Report Reasons

    [HttpGet("report-reasons")]
    [Authorize(Policy = AdminPermissions.ReportView)]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ReportReasonResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ReportReasonResponse>>>> GetReportReasons(
        [FromQuery] TargetType? targetType,
        CancellationToken cancellationToken)
    {
        var result = await _moderationService.GetReportReasonsAsync(targetType, cancellationToken);
        return Ok(ApiResponse<IReadOnlyList<ReportReasonResponse>>.SuccessResponse(result));
    }
}
