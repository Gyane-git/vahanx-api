using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.DTOs.Admin;
using VahanX.Application.Services;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Read-only permission listing. Permissions are system-defined.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/admin/permissions")]
[ApiVersion("1.0")]
public class AdminPermissionsController : ControllerBase
{
    private readonly IPermissionManagementService _service;

    public AdminPermissionsController(IPermissionManagementService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = AdminPermissions.PermissionView)]
    public async Task<ActionResult<ApiResponse<List<PermissionDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<List<PermissionDto>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AdminPermissions.PermissionView)]
    public async Task<ActionResult<ApiResponse<PermissionDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Permission not found."));
        return Ok(ApiResponse<PermissionDto>.SuccessResponse(result));
    }
}
