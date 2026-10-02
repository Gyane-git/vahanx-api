using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Admin;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for admin dashboard operations.
/// </summary>
[ApiController]
[Authorize(Policy = AdminPermissions.AnalyticsView)]
[Route("api/v{version:apiVersion}/admin")]
[ApiVersion("1.0")]
public class AdminController : ControllerBase
{
    private readonly IAdminDashboardService _dashboardService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminDashboardService dashboardService, ILogger<AdminController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    /// <summary>
    /// Get admin dashboard summary with high-level metrics.
    /// </summary>
    [HttpGet("dashboard/summary")]
    [ProducesResponseType(typeof(ApiResponse<AdminDashboardSummaryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<AdminDashboardSummaryDto>>> GetDashboardSummary(CancellationToken cancellationToken)
    {
        var result = await _dashboardService.GetDashboardSummaryAsync(cancellationToken);
        return Ok(ApiResponse<AdminDashboardSummaryDto>.SuccessResponse(result));
    }
}
