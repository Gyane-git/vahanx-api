using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.DTOs.Admin;
using VahanX.Application.Services;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Admin role management endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/admin/roles")]
[ApiVersion("1.0")]
public class AdminRolesController : ControllerBase
{
    private readonly IRoleManagementService _service;

    public AdminRolesController(IRoleManagementService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = AdminPermissions.RoleView)]
    public async Task<ActionResult<ApiResponse<List<RoleDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<List<RoleDto>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AdminPermissions.RoleView)]
    public async Task<ActionResult<ApiResponse<RoleDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Role not found."));
        return Ok(ApiResponse<RoleDto>.SuccessResponse(result));
    }

    [HttpPost]
    [Authorize(Policy = AdminPermissions.RoleCreate)]
    public async Task<ActionResult<ApiResponse<RoleDto>>> Create([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, GetActorId(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<RoleDto>.SuccessResponse(result, "Role created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AdminPermissions.RoleUpdate)]
    public async Task<ActionResult<ApiResponse<RoleDto>>> Update(Guid id, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, GetActorId(), cancellationToken);
        return Ok(ApiResponse<RoleDto>.SuccessResponse(result, "Role updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AdminPermissions.RoleDelete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, GetActorId(), cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}/permissions")]
    [Authorize(Policy = AdminPermissions.RoleView)]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetPermissions(Guid id, CancellationToken cancellationToken)
    {
        var permissions = await _service.GetRolePermissionsAsync(id, cancellationToken);
        return Ok(ApiResponse<List<string>>.SuccessResponse(permissions));
    }

    [HttpPost("{id:guid}/permissions")]
    [Authorize(Policy = AdminPermissions.RolePermissionAssign)]
    public async Task<ActionResult<ApiResponse<object>>> AssignPermission(Guid id, [FromBody] AssignPermissionRequest request, CancellationToken cancellationToken)
    {
        await _service.AssignPermissionAsync(id, request.PermissionId, GetActorId(), cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "Permission assigned."));
    }

    [HttpDelete("{id:guid}/permissions/{permissionId:guid}")]
    [Authorize(Policy = AdminPermissions.RolePermissionAssign)]
    public async Task<IActionResult> RemovePermission(Guid id, Guid permissionId, CancellationToken cancellationToken)
    {
        await _service.RemovePermissionAsync(id, permissionId, GetActorId(), cancellationToken);
        return NoContent();
    }

    private Guid? GetActorId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
