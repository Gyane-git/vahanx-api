using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VahanX.Application.Common;
using VahanX.Application.DTOs.Admin;
using VahanX.Application.Services;

namespace VahanX.Api.Controllers.V1;

/// <summary>
/// Admin user management endpoints.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/admin/users")]
[ApiVersion("1.0")]
public class AdminUsersController : ControllerBase
{
    private readonly IUserManagementService _service;

    public AdminUsersController(IUserManagementService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = AdminPermissions.UserView)]
    public async Task<ActionResult<ApiResponse<PagedResult<UserListItemDto>>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetAllAsync(page, pageSize, search, isActive, cancellationToken);
        return Ok(ApiResponse<PagedResult<UserListItemDto>>.SuccessResponse(result));
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AdminPermissions.UserView)]
    public async Task<ActionResult<ApiResponse<UserDetailDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound(ApiResponse<object>.ErrorResponse("User not found."));
        return Ok(ApiResponse<UserDetailDto>.SuccessResponse(result));
    }

    [HttpPost]
    [Authorize(Policy = AdminPermissions.UserCreate)]
    public async Task<ActionResult<ApiResponse<UserDetailDto>>> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, GetActorId(), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<UserDetailDto>.SuccessResponse(result, "User created successfully."));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AdminPermissions.UserUpdate)]
    public async Task<ActionResult<ApiResponse<UserDetailDto>>> Update(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, GetActorId(), cancellationToken);
        return Ok(ApiResponse<UserDetailDto>.SuccessResponse(result, "User updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AdminPermissions.UserDelete)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, GetActorId(), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/activate")]
    [Authorize(Policy = AdminPermissions.UserActivate)]
    public async Task<ActionResult<ApiResponse<object>>> Activate(Guid id, CancellationToken cancellationToken)
    {
        await _service.ActivateAsync(id, GetActorId(), cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "User activated."));
    }

    [HttpPost("{id:guid}/deactivate")]
    [Authorize(Policy = AdminPermissions.UserDeactivate)]
    public async Task<ActionResult<ApiResponse<object>>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeactivateAsync(id, GetActorId(), cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "User deactivated."));
    }

    [HttpPost("{id:guid}/suspend")]
    [Authorize(Policy = AdminPermissions.UserSuspend)]
    public async Task<ActionResult<ApiResponse<object>>> Suspend(Guid id, [FromBody] SuspendUserRequest request, CancellationToken cancellationToken)
    {
        await _service.SuspendAsync(id, request.Reason, GetActorId(), cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "User suspended."));
    }

    [HttpGet("{id:guid}/roles")]
    [Authorize(Policy = AdminPermissions.UserView)]
    public async Task<ActionResult<ApiResponse<List<string>>>> GetRoles(Guid id, CancellationToken cancellationToken)
    {
        var roles = await _service.GetUserRolesAsync(id, cancellationToken);
        return Ok(ApiResponse<List<string>>.SuccessResponse(roles));
    }

    [HttpPost("{id:guid}/roles")]
    [Authorize(Policy = AdminPermissions.UserRoleAssign)]
    public async Task<ActionResult<ApiResponse<object>>> AssignRole(Guid id, [FromBody] AssignRoleRequest request, CancellationToken cancellationToken)
    {
        await _service.AssignRoleAsync(id, request.RoleId, GetActorId(), cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(new { }, "Role assigned."));
    }

    [HttpDelete("{id:guid}/roles/{roleId:guid}")]
    [Authorize(Policy = AdminPermissions.UserRoleAssign)]
    public async Task<IActionResult> RemoveRole(Guid id, Guid roleId, CancellationToken cancellationToken)
    {
        await _service.RemoveRoleAsync(id, roleId, GetActorId(), cancellationToken);
        return NoContent();
    }

    private Guid? GetActorId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
