using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Admin;
using VahanX.Application.DTOs.Audit;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

public interface IUserManagementService
{
    Task<PagedResult<UserListItemDto>> GetAllAsync(int page, int pageSize, string? search, bool? isActive, CancellationToken cancellationToken = default);
    Task<UserDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserDetailDto> CreateAsync(CreateUserRequest request, Guid? actorId, CancellationToken cancellationToken = default);
    Task<UserDetailDto> UpdateAsync(Guid id, UpdateUserRequest request, Guid? actorId, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default);
    Task ActivateAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default);
    Task DeactivateAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default);
    Task SuspendAsync(Guid id, string reason, Guid? actorId, CancellationToken cancellationToken = default);
    Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AssignRoleAsync(Guid userId, Guid roleId, Guid? actorId, CancellationToken cancellationToken = default);
    Task RemoveRoleAsync(Guid userId, Guid roleId, Guid? actorId, CancellationToken cancellationToken = default);
}

public class UserManagementService : IUserManagementService
{
    private readonly IRepository<User> _users;
    private readonly IRepository<UserRole> _userRoles;
    private readonly IRepository<Role> _roles;
    private readonly IRepository<RolePermission> _rolePermissions;
    private readonly IRepository<Permission> _permissions;
    private readonly IRepository<UserRestriction> _restrictions;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IAuditService _auditService;
    private readonly PasswordPolicySettings _passwordPolicy;

    public UserManagementService(
        IRepository<User> users,
        IRepository<UserRole> userRoles,
        IRepository<Role> roles,
        IRepository<RolePermission> rolePermissions,
        IRepository<Permission> permissions,
        IRepository<UserRestriction> restrictions,
        IPasswordHasher<User> passwordHasher,
        IAuditService auditService,
        IConfiguration configuration)
    {
        _users = users;
        _userRoles = userRoles;
        _roles = roles;
        _rolePermissions = rolePermissions;
        _permissions = permissions;
        _restrictions = restrictions;
        _passwordHasher = passwordHasher;
        _auditService = auditService;
        _passwordPolicy = configuration.GetSection(PasswordPolicySettings.SectionName).Get<PasswordPolicySettings>() ?? new PasswordPolicySettings();
    }

    public async Task<PagedResult<UserListItemDto>> GetAllAsync(int page, int pageSize, string? search, bool? isActive, CancellationToken cancellationToken = default)
    {
        var query = await _users.QueryAsync(true, cancellationToken);

        if (isActive.HasValue)
            query = query.Where(u => u.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(u => u.Email.ToLower().Contains(s) || u.FirstName.ToLower().Contains(s) || u.LastName.ToLower().Contains(s));
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query.OrderByDescending(u => u.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        var result = new List<UserListItemDto>();
        foreach (var u in items)
        {
            result.Add(new UserListItemDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                IsActive = u.IsActive,
                EmailConfirmed = u.EmailConfirmed,
                LastLoginAt = u.LastLoginAt,
                Roles = await GetRoleNamesAsync(u.Id, cancellationToken)
            });
        }

        return PagedResult<UserListItemDto>.Create(result, total, page, pageSize);
    }

    public async Task<UserDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken);
        if (user is null) return null;
        var roles = await GetRoleNamesAsync(id, cancellationToken);
        var permissions = await GetPermissionNamesAsync(id, cancellationToken);
        return new UserDetailDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed,
            LastLoginAt = user.LastLoginAt,
            Roles = roles,
            Permissions = permissions,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<UserDetailDto> CreateAsync(CreateUserRequest request, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var users = await _users.QueryAsync(true, cancellationToken);
        if (await users.AnyAsync(u => u.Email.ToLower() == email, cancellationToken))
            throw new ConflictException($"A user with email '{email}' already exists.");

        ValidatePassword(request.Password);

        if (request.RoleIds.Count > 0)
        {
            var roleQuery = await _roles.QueryAsync(true, cancellationToken);
            var count = await roleQuery.CountAsync(r => request.RoleIds.Contains(r.Id), cancellationToken);
            if (count != request.RoleIds.Count)
                throw new NotFoundException("One or more roles were not found.");
        }

        var user = new User
        {
            Email = email,
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = request.IsActive,
            EmailConfirmed = request.EmailConfirmed
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _users.AddAsync(user, cancellationToken);

        foreach (var roleId in request.RoleIds.Distinct())
            await _userRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = roleId }, cancellationToken);

        await AuditAsync(actorId, AuditAction.Create, "User", user.Id.ToString(), $"Created user {email}", cancellationToken);

        return (await GetByIdAsync(user.Id, cancellationToken))!;
    }

    public async Task<UserDetailDto> UpdateAsync(Guid id, UpdateUserRequest request, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User not found.");
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.EmailConfirmed = request.EmailConfirmed;
        await _users.UpdateAsync(user, cancellationToken);
        await AuditAsync(actorId, AuditAction.Update, "User", id.ToString(), "Updated user details", cancellationToken);
        return (await GetByIdAsync(id, cancellationToken))!;
    }

    public async Task DeleteAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User not found.");
        if (actorId == id)
            throw new ConflictException("You cannot delete your own account.");
        await _users.DeleteAsync(user, cancellationToken);
        await AuditAsync(actorId, AuditAction.Delete, "User", id.ToString(), $"Deleted user {user.Email}", cancellationToken);
    }

    public async Task ActivateAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User not found.");
        user.IsActive = true;
        await _users.UpdateAsync(user, cancellationToken);

        var q = await _restrictions.QueryAsync(false, cancellationToken);
        var activeRestrictions = await q.Where(r => r.UserId == id && r.IsActive && r.RestrictionType == UserRestrictionType.FullPlatformRestriction).ToListAsync(cancellationToken);
        foreach (var r in activeRestrictions)
        {
            r.IsActive = false;
            r.EndsAt ??= DateTime.UtcNow;
            await _restrictions.UpdateAsync(r, cancellationToken);
        }

        await AuditAsync(actorId, AuditAction.Update, "User", id.ToString(), "User activated", cancellationToken);
    }

    public async Task DeactivateAsync(Guid id, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User not found.");
        if (actorId == id)
            throw new ConflictException("You cannot deactivate your own account.");
        user.IsActive = false;
        await _users.UpdateAsync(user, cancellationToken);
        await AuditAsync(actorId, AuditAction.Update, "User", id.ToString(), "User deactivated", cancellationToken);
    }

    public async Task SuspendAsync(Guid id, string reason, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("User not found.");
        if (actorId == id)
            throw new ConflictException("You cannot suspend your own account.");

        await _restrictions.AddAsync(new UserRestriction
        {
            UserId = id,
            RestrictionType = UserRestrictionType.FullPlatformRestriction,
            Reason = reason,
            StartsAt = DateTime.UtcNow,
            IsActive = true,
            CreatedByUserId = actorId ?? Guid.Empty
        }, cancellationToken);

        await AuditAsync(actorId, AuditAction.Suspend, "User", id.ToString(), $"User suspended: {reason}", cancellationToken);
    }

    public async Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        if (await _users.GetByIdAsync(userId, cancellationToken) is null)
            throw new NotFoundException("User not found.");
        return await GetRoleNamesAsync(userId, cancellationToken);
    }

    public async Task AssignRoleAsync(Guid userId, Guid roleId, Guid? actorId, CancellationToken cancellationToken = default)
    {
        if (await _users.GetByIdAsync(userId, cancellationToken) is null)
            throw new NotFoundException("User not found.");
        if (await _roles.GetByIdAsync(roleId, cancellationToken) is null)
            throw new NotFoundException("Role not found.");

        var urQuery = await _userRoles.QueryAsync(true, cancellationToken);
        if (await urQuery.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken))
            throw new ConflictException("Role is already assigned to this user.");

        await _userRoles.AddAsync(new UserRole { UserId = userId, RoleId = roleId }, cancellationToken);
        await AuditAsync(actorId, AuditAction.RoleChanged, "User", userId.ToString(), $"Assigned role {roleId}", cancellationToken);
    }

    public async Task RemoveRoleAsync(Guid userId, Guid roleId, Guid? actorId, CancellationToken cancellationToken = default)
    {
        var urQuery = await _userRoles.QueryAsync(false, cancellationToken);
        var link = await urQuery.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);
        if (link is null)
            throw new NotFoundException("Role assignment not found.");

        await _userRoles.DeleteAsync(link, cancellationToken);
        await AuditAsync(actorId, AuditAction.RoleChanged, "User", userId.ToString(), $"Removed role {roleId}", cancellationToken);
    }

    private async Task<List<string>> GetRoleNamesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var urQuery = await _userRoles.QueryAsync(true, cancellationToken);
        var roleIds = await urQuery.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync(cancellationToken);
        var roleQuery = await _roles.QueryAsync(true, cancellationToken);
        return await roleQuery.Where(r => roleIds.Contains(r.Id)).Select(r => r.Name).ToListAsync(cancellationToken);
    }

    private async Task<List<string>> GetPermissionNamesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var urQuery = await _userRoles.QueryAsync(true, cancellationToken);
        var roleIds = await urQuery.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync(cancellationToken);
        var rpQuery = await _rolePermissions.QueryAsync(true, cancellationToken);
        var permissionIds = await rpQuery.Where(rp => roleIds.Contains(rp.RoleId)).Select(rp => rp.PermissionId).Distinct().ToListAsync(cancellationToken);
        var pQuery = await _permissions.QueryAsync(true, cancellationToken);
        return await pQuery.Where(p => permissionIds.Contains(p.Id)).Select(p => p.Name).ToListAsync(cancellationToken);
    }

    private void ValidatePassword(string password)
    {
        if (password.Length < _passwordPolicy.MinimumLength)
            throw new ValidationException("Invalid password.", new Dictionary<string, string[]> { ["Password"] = [$"Password must be at least {_passwordPolicy.MinimumLength} characters."] });
        if (_passwordPolicy.RequireUppercase && !password.Any(char.IsUpper))
            throw new ValidationException("Invalid password.", new Dictionary<string, string[]> { ["Password"] = ["Password must contain an uppercase letter."] });
        if (_passwordPolicy.RequireLowercase && !password.Any(char.IsLower))
            throw new ValidationException("Invalid password.", new Dictionary<string, string[]> { ["Password"] = ["Password must contain a lowercase letter."] });
        if (_passwordPolicy.RequireDigit && !password.Any(char.IsDigit))
            throw new ValidationException("Invalid password.", new Dictionary<string, string[]> { ["Password"] = ["Password must contain a digit."] });
        if (_passwordPolicy.RequireNonAlphanumeric && !password.Any(c => !char.IsLetterOrDigit(c)))
            throw new ValidationException("Invalid password.", new Dictionary<string, string[]> { ["Password"] = ["Password must contain a non-alphanumeric character."] });
    }

    private async Task AuditAsync(Guid? actorId, AuditAction action, string entityType, string entityId, string change, CancellationToken cancellationToken)
    {
        await _auditService.AuditAsync(new CreateAuditLogRequest
        {
            ActorUserId = actorId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Changes = change
        }, cancellationToken);
    }
}
