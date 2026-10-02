using Microsoft.EntityFrameworkCore;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Admin;
using VahanX.Application.DTOs.Audit;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

public interface IRoleManagementService
{
    Task<List<RoleDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<RoleDto> CreateAsync(CreateRoleRequest request, Guid? actorId, CancellationToken cancellationToken = default);
    Task<RoleDto> UpdateAsync(Guid id, UpdateRoleRequest request, Guid? actorId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default);
    Task<List<string>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task AssignPermissionAsync(Guid roleId, Guid permissionId, Guid? actorId, CancellationToken cancellationToken = default);
    Task RemovePermissionAsync(Guid roleId, Guid permissionId, Guid? actorId, CancellationToken cancellationToken = default);
}

public class RoleManagementService : IRoleManagementService
{
    private readonly IRepository<Role> _roles;
    private readonly IRepository<RolePermission> _rolePermissions;
    private readonly IRepository<Permission> _permissions;
    private readonly IRepository<UserRole> _userRoles;
    private readonly IAuditService _auditService;

    public RoleManagementService(
        IRepository<Role> roles,
        IRepository<RolePermission> rolePermissions,
        IRepository<Permission> permissions,
        IRepository<UserRole> userRoles,
        IAuditService auditService)
    {
        _roles = roles;
        _rolePermissions = rolePermissions;
        _permissions = permissions;
        _userRoles = userRoles;
        _auditService = auditService;
    }

    public async Task<List<RoleDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = await _roles.QueryAsync(true, cancellationToken);
        var roles = await query.OrderBy(r => r.Name).ToListAsync(cancellationToken);
        var result = new List<RoleDto>();
        foreach (var r in roles)
            result.Add(await MapAsync(r, cancellationToken));
        return result;
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken);
        return role is null ? null : await MapAsync(role, cancellationToken);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleRequest request, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var name = request.Name.Trim();
        var query = await _roles.QueryAsync(true, cancellationToken);
        if (await query.AnyAsync(r => r.Name.ToLower() == name.ToLower(), cancellationToken))
            throw new ConflictException($"Role '{name}' already exists.");

        var role = new Role { Name = name, Description = request.Description, IsActive = request.IsActive, IsSystemRole = false };
        await _roles.AddAsync(role, cancellationToken);
        await AuditAsync(actorId, AuditAction.Create, role.Id, $"Created role {name}", cancellationToken);
        return await MapAsync(role, cancellationToken);
    }

    public async Task<RoleDto> UpdateAsync(Guid id, UpdateRoleRequest request, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Role not found.");
        if (role.IsSystemRole)
            throw new ForbiddenException("System roles cannot be modified.");

        role.Description = request.Description;
        role.IsActive = request.IsActive;
        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != role.Name)
        {
            var query = await _roles.QueryAsync(true, cancellationToken);
            if (await query.AnyAsync(r => r.Name.ToLower() == request.Name.Trim().ToLower() && r.Id != id, cancellationToken))
                throw new ConflictException($"Role '{request.Name}' already exists.");
            role.Name = request.Name.Trim();
        }

        await _roles.UpdateAsync(role, cancellationToken);
        await AuditAsync(actorId, AuditAction.Update, role.Id, "Updated role", cancellationToken);
        return await MapAsync(role, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Role not found.");
        if (role.IsSystemRole)
            throw new ForbiddenException("System roles cannot be deleted.");

        var urQuery = await _userRoles.QueryAsync(true, cancellationToken);
        if (await urQuery.AnyAsync(ur => ur.RoleId == id, cancellationToken))
            throw new ConflictException("Role is assigned to users. Remove assignments before deleting.");

        await _roles.DeleteAsync(role, cancellationToken);
        await AuditAsync(actorId, AuditAction.Delete, id, $"Deleted role {role.Name}", cancellationToken);
    }

    public async Task<List<string>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        if (await _roles.GetByIdAsync(roleId, cancellationToken) is null)
            throw new NotFoundException("Role not found.");
        return await GetPermissionNamesAsync(roleId, cancellationToken);
    }

    public async Task AssignPermissionAsync(Guid roleId, Guid permissionId, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var role = await _roles.GetByIdAsync(roleId, cancellationToken) ?? throw new NotFoundException("Role not found.");
        if (role.IsSystemRole)
            throw new ForbiddenException("Permissions of system roles cannot be modified.");
        if (await _permissions.GetByIdAsync(permissionId, cancellationToken) is null)
            throw new NotFoundException("Permission not found.");

        var rpQuery = await _rolePermissions.QueryAsync(true, cancellationToken);
        if (await rpQuery.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken))
            throw new ConflictException("Permission is already assigned to this role.");

        await _rolePermissions.AddAsync(new RolePermission { RoleId = roleId, PermissionId = permissionId }, cancellationToken);
        await AuditAsync(actorId, AuditAction.PermissionChanged, roleId, $"Assigned permission {permissionId}", cancellationToken);
    }

    public async Task RemovePermissionAsync(Guid roleId, Guid permissionId, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var roleForRemove = await _roles.GetByIdAsync(roleId, cancellationToken);
        if (roleForRemove?.IsSystemRole == true)
            throw new ForbiddenException("Permissions of system roles cannot be modified.");
        var rpQuery = await _rolePermissions.QueryAsync(false, cancellationToken);
        var link = await rpQuery.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);
        if (link is null)
            throw new NotFoundException("Permission assignment not found.");

        await _rolePermissions.DeleteAsync(link, cancellationToken);
        await AuditAsync(actorId, AuditAction.PermissionChanged, roleId, $"Removed permission {permissionId}", cancellationToken);
    }

    private async Task<List<string>> GetPermissionNamesAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var rpQuery = await _rolePermissions.QueryAsync(true, cancellationToken);
        var permissionIds = await rpQuery.Where(rp => rp.RoleId == roleId).Select(rp => rp.PermissionId).ToListAsync(cancellationToken);
        var pQuery = await _permissions.QueryAsync(true, cancellationToken);
        return await pQuery.Where(p => permissionIds.Contains(p.Id)).Select(p => p.Name).ToListAsync(cancellationToken);
    }

    private async Task<RoleDto> MapAsync(Role role, CancellationToken cancellationToken) => new()
    {
        Id = role.Id,
        Name = role.Name,
        Description = role.Description,
        IsSystemRole = role.IsSystemRole,
        IsActive = role.IsActive,
        Permissions = await GetPermissionNamesAsync(role.Id, cancellationToken)
    };

    private async Task AuditAsync(Guid? actorId, AuditAction action, Guid roleId, string change, CancellationToken cancellationToken)
    {
        await _auditService.AuditAsync(new CreateAuditLogRequest
        {
            ActorUserId = actorId,
            Action = action,
            EntityType = "Role",
            EntityId = roleId.ToString(),
            Changes = change
        }, cancellationToken);
    }
}
