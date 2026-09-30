using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Audit;
using VahanX.Domain.Enums;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Controller for audit log operations.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/admin/audit")]
[ApiVersion("1.0")]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;
    private readonly ILogger<AuditController> _logger;

    public AuditController(IAuditService auditService, ILogger<AuditController> logger)
    {
        _auditService = auditService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<AuditLogResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<AuditLogResponse>>>> GetAuditLogs(
        [FromQuery] Guid? actorUserId,
        [FromQuery] string? entityType,
        [FromQuery] string? entityId,
        [FromQuery] AuditAction? action,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] AuditResult? result,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var filters = new AuditLogQueryParams
        {
            ActorUserId = actorUserId,
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            From = from,
            To = to,
            Result = result,
            Page = page,
            PageSize = pageSize
        };

        var result2 = await _auditService.GetAuditLogsAsync(filters, cancellationToken);
        return Ok(ApiResponse<PagedResult<AuditLogResponse>>.SuccessResponse(result2));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AuditLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AuditLogResponse>>> GetAuditLogById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _auditService.GetAuditLogByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Audit log not found."));
        return Ok(ApiResponse<AuditLogResponse>.SuccessResponse(result));
    }
}
