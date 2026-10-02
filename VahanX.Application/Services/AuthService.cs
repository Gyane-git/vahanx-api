using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VahanX.Application.Common;
using VahanX.Application.Common.Interfaces;
using VahanX.Application.DTOs.Admin;
using VahanX.Application.DTOs.Audit;
using VahanX.Application.DTOs.Auth;
using VahanX.Domain.Entities;
using VahanX.Domain.Enums;
using VahanX.Domain.Exceptions;

namespace VahanX.Application.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);
    Task<RefreshTokenResponse> RefreshAsync(RefreshTokenRequest request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);
    Task LogoutAsync(RefreshTokenRequest request, Guid? actorUserId, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);
    Task<CurrentUserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Authentication service.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IRepository<User> _users;
    private readonly IRepository<UserRole> _userRoles;
    private readonly IRepository<Role> _roles;
    private readonly IRepository<RolePermission> _rolePermissions;
    private readonly IRepository<Permission> _permissions;
    private readonly IRepository<RefreshToken> _refreshTokens;
    private readonly IRepository<LoginHistory> _loginHistory;
    private readonly IRepository<UserRestriction> _restrictions;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IAuditService _auditService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IRepository<User> users,
        IRepository<UserRole> userRoles,
        IRepository<Role> roles,
        IRepository<RolePermission> rolePermissions,
        IRepository<Permission> permissions,
        IRepository<RefreshToken> refreshTokens,
        IRepository<LoginHistory> loginHistory,
        IRepository<UserRestriction> restrictions,
        ITokenService tokenService,
        IPasswordHasher<User> passwordHasher,
        IAuditService auditService,
        ILogger<AuthService> logger)
    {
        _users = users;
        _userRoles = userRoles;
        _roles = roles;
        _rolePermissions = rolePermissions;
        _permissions = permissions;
        _refreshTokens = refreshTokens;
        _loginHistory = loginHistory;
        _restrictions = restrictions;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _auditService = auditService;
        _logger = logger;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var userQuery = await _users.QueryAsync(true, cancellationToken);
        var user = await userQuery.FirstOrDefaultAsync(u => u.Email.ToLower() == email, cancellationToken);

        if (user is null)
        {
            await RecordLoginEventAsync(null, email, LoginEventType.LoginFailed, false, "INVALID_CREDENTIALS", ipAddress, userAgent, cancellationToken);
            throw new UnauthorizedException("Invalid email or password.");
        }

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verify == PasswordVerificationResult.Failed || string.IsNullOrEmpty(user.PasswordHash))
        {
            await RecordLoginEventAsync(user.Id, email, LoginEventType.LoginFailed, false, "INVALID_CREDENTIALS", ipAddress, userAgent, cancellationToken);
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            await RecordLoginEventAsync(user.Id, email, LoginEventType.AccountBlocked, false, "ACCOUNT_INACTIVE", ipAddress, userAgent, cancellationToken);
            throw new UnauthorizedException("Account is inactive. Please contact support.");
        }

        var isSuspended = await IsSuspendedAsync(user.Id, cancellationToken);
        if (isSuspended)
        {
            await RecordLoginEventAsync(user.Id, email, LoginEventType.AccountBlocked, false, "ACCOUNT_SUSPENDED", ipAddress, userAgent, cancellationToken);
            throw new UnauthorizedException("Account is suspended. Please contact support.");
        }

        var (roles, permissions) = await GetAccessProfileAsync(user.Id, cancellationToken);

        var accessToken = _tokenService.CreateAccessToken(user, roles, permissions, out var expiresAt);
        var refreshTokenRaw = _tokenService.GenerateRefreshToken();
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = _tokenService.HashToken(refreshTokenRaw),
            ExpiresAt = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpirationDays),
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
        await _refreshTokens.AddAsync(refreshTokenEntity, cancellationToken);

        user.LastLoginAt = DateTime.UtcNow;
        await _users.UpdateAsync(user, cancellationToken);

        await RecordLoginEventAsync(user.Id, email, LoginEventType.Login, true, null, ipAddress, userAgent, cancellationToken);
        await _auditService.AuditAsync(new CreateAuditLogRequest
        {
            ActorUserId = user.Id,
            Action = AuditAction.Login,
            EntityType = "User",
            EntityId = user.Id.ToString(),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            Result = AuditResult.Success
        }, cancellationToken);

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenRaw,
            AccessTokenExpiresAt = expiresAt,
            RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt,
            User = BuildUserInfo(user, roles, permissions)
        };
    }

    public async Task<RefreshTokenResponse> RefreshAsync(RefreshTokenRequest request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default)
    {
        var tokenHash = _tokenService.HashToken(request.RefreshToken);
        var tokenQuery = await _refreshTokens.QueryAsync(false, cancellationToken);
        var stored = await tokenQuery.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

        if (stored is null)
            throw new UnauthorizedException("Invalid refresh token.");

        if (stored.RevokedAt is not null)
        {
            // Reuse of a revoked/rotated token: revoke the whole session family defensively.
            var userTokensQuery = await _refreshTokens.QueryAsync(false, cancellationToken);
            var userTokens = await userTokensQuery.Where(t => t.UserId == stored.UserId && t.RevokedAt == null).ToListAsync(cancellationToken);
            foreach (var t in userTokens)
            {
                t.RevokedAt = DateTime.UtcNow;
                await _refreshTokens.UpdateAsync(t, cancellationToken);
            }
            throw new UnauthorizedException("Refresh token has been revoked.");
        }

        if (stored.ExpiresAt <= DateTime.UtcNow)
            throw new UnauthorizedException("Refresh token has expired.");

        var user = await _users.GetByIdAsync(stored.UserId, cancellationToken);
        if (user is null || !user.IsActive || await IsSuspendedAsync(user.Id, cancellationToken))
            throw new UnauthorizedException("Account is not active.");

        var (roles, permissions) = await GetAccessProfileAsync(user.Id, cancellationToken);
        var accessToken = _tokenService.CreateAccessToken(user, roles, permissions, out var expiresAt);

        // Rotate
        var newRaw = _tokenService.GenerateRefreshToken();
        stored.RevokedAt = DateTime.UtcNow;
        stored.ReplacedByTokenHash = _tokenService.HashToken(newRaw);
        await _refreshTokens.UpdateAsync(stored, cancellationToken);

        var newToken = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = _tokenService.HashToken(newRaw),
            ExpiresAt = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpirationDays),
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
        await _refreshTokens.AddAsync(newToken, cancellationToken);

        await RecordLoginEventAsync(user.Id, user.Email, LoginEventType.Refresh, true, null, ipAddress, userAgent, cancellationToken);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRaw,
            AccessTokenExpiresAt = expiresAt,
            RefreshTokenExpiresAt = newToken.ExpiresAt
        };
    }

    public async Task LogoutAsync(RefreshTokenRequest request, Guid? actorUserId, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default)
    {
        var tokenHash = _tokenService.HashToken(request.RefreshToken);
        var tokenQuery = await _refreshTokens.QueryAsync(false, cancellationToken);
        var stored = await tokenQuery.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);

        if (stored is not null && stored.RevokedAt is null)
        {
            stored.RevokedAt = DateTime.UtcNow;
            await _refreshTokens.UpdateAsync(stored, cancellationToken);
        }

        var userId = actorUserId ?? stored?.UserId;
        if (userId.HasValue)
        {
            await RecordLoginEventAsync(userId, string.Empty, LoginEventType.Logout, true, null, ipAddress, userAgent, cancellationToken);
            await _auditService.AuditAsync(new CreateAuditLogRequest
            {
                ActorUserId = userId,
                Action = AuditAction.Logout,
                EntityType = "User",
                EntityId = userId.ToString(),
                IpAddress = ipAddress,
                UserAgent = userAgent
            }, cancellationToken);
        }
    }

    public async Task<CurrentUserDto> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            throw new NotFoundException("User not found.");

        var (roles, permissions) = await GetAccessProfileAsync(userId, cancellationToken);
        return BuildUserInfo(user, roles, permissions);
    }

    private async Task<(List<string> Roles, List<string> Permissions)> GetAccessProfileAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userRoleQuery = await _userRoles.QueryAsync(true, cancellationToken);
        var roleIds = await userRoleQuery.Where(ur => ur.UserId == userId).Select(ur => ur.RoleId).ToListAsync(cancellationToken);

        var roleQuery = await _roles.QueryAsync(true, cancellationToken);
        var roles = await roleQuery.Where(r => roleIds.Contains(r.Id) && r.IsActive).Select(r => r.Name).ToListAsync(cancellationToken);

        var rpQuery = await _rolePermissions.QueryAsync(true, cancellationToken);
        var permissionIds = await rpQuery.Where(rp => roleIds.Contains(rp.RoleId)).Select(rp => rp.PermissionId).Distinct().ToListAsync(cancellationToken);

        var pQuery = await _permissions.QueryAsync(true, cancellationToken);
        var permissions = await pQuery.Where(p => permissionIds.Contains(p.Id)).Select(p => p.Name).ToListAsync(cancellationToken);

        return (roles, permissions);
    }

    private async Task<bool> IsSuspendedAsync(Guid userId, CancellationToken cancellationToken)
    {
        var q = await _restrictions.QueryAsync(true, cancellationToken);
        var now = DateTime.UtcNow;
        return await q.AnyAsync(r => r.UserId == userId
            && r.IsActive
            && r.RestrictionType == UserRestrictionType.FullPlatformRestriction
            && r.StartsAt <= now
            && (r.EndsAt == null || r.EndsAt > now), cancellationToken);
    }

    private async Task RecordLoginEventAsync(Guid? userId, string email, LoginEventType eventType, bool success, string? failureReason, string? ipAddress, string? userAgent, CancellationToken cancellationToken)
    {
        try
        {
            await _loginHistory.AddAsync(new LoginHistory
            {
                UserId = userId,
                Email = email,
                EventType = eventType,
                Success = success,
                FailureReason = failureReason,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                Timestamp = DateTime.UtcNow
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to record login history event {EventType}", eventType);
        }
    }

    private static CurrentUserDto BuildUserInfo(User user, List<string> roles, List<string> permissions) => new()
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
        Permissions = permissions
    };
}
