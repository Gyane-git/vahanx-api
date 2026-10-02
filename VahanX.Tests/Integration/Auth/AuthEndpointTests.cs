using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using VahanX.Application.DTOs.Auth;
using VahanX.Application.Common;
using VahanX.Domain.Entities;
using VahanX.Infrastructure.Persistence;

namespace VahanX.Tests.Integration.Auth;

public class AuthEndpointTests : IClassFixture<AuthTestFactory>
{
    private readonly AuthTestFactory _factory;

    public AuthEndpointTests(AuthTestFactory factory)
    {
        _factory = factory;
    }

    private HttpClient NewClient() => _factory.CreateClient();

    private async Task<string> LoginAsync(HttpClient client, string email = "admin@vahanx.com", string password = "VahanX@Dev123!")
    {
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        return body!.Data!.AccessToken;
    }

    [Fact]
    public async Task ValidLogin_ReturnsTokensAndUserInfo()
    {
        var client = NewClient();
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = "admin@vahanx.com", password = "VahanX@Dev123!" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        Assert.NotNull(body?.Data);
        Assert.False(string.IsNullOrEmpty(body.Data.AccessToken));
        Assert.False(string.IsNullOrEmpty(body.Data.RefreshToken));
        Assert.Contains("SuperAdmin", body.Data.User.Roles);
        Assert.NotEmpty(body.Data.User.Permissions);
        Assert.DoesNotContain("PasswordHash", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task InvalidLogin_Returns401()
    {
        var client = NewClient();
        var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = "admin@vahanx.com", password = "WrongPassword1!" });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task InactiveUserLogin_Returns401()
    {
        // Deactivate the seeded admin, attempt login, re-activate.
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
            var user = context.Users.First(u => u.Email == "admin@vahanx.com");
            user.IsActive = false;
            await context.SaveChangesAsync();

            try
            {
                var client = NewClient();
                var response = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = "admin@vahanx.com", password = "VahanX@Dev123!" });
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
            finally
            {
                user.IsActive = true;
                await context.SaveChangesAsync();
            }
        }
    }

    [Fact]
    public async Task Me_WithValidToken_ReturnsCurrentUser()
    {
        var client = NewClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/v1/auth/me");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ApiResponse<CurrentUserDto>>();
        Assert.Equal("admin@vahanx.com", body!.Data!.Email);
        Assert.Contains("SuperAdmin", body.Data.Roles);
    }

    [Fact]
    public async Task Me_WithoutToken_Returns401()
    {
        var client = NewClient();
        var response = await client.GetAsync("/api/v1/auth/me");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ExpiredToken_Returns401()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DEV-ONLY-vahanx-super-secret-signing-key-change-me-in-prod-0123456789"));
        var token = new JwtSecurityToken(
            issuer: "VahanX",
            audience: "VahanX.Admin",
            claims: [new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())],
            expires: DateTime.UtcNow.AddMinutes(-30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        var client = NewClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));

        var response = await client.GetAsync("/api/v1/auth/me");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Refresh_RotatesTokens_OldTokensBecomeRejected()
    {
        var client = NewClient();
        var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = "admin@vahanx.com", password = "VahanX@Dev123!" });
        var body = await login.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        var oldRefresh = body!.Data!.RefreshToken;

        var refreshResponse = await client.PostAsJsonAsync("/api/v1/auth/refresh", new { refreshToken = oldRefresh });
        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
        var refreshed = await refreshResponse.Content.ReadFromJsonAsync<ApiResponse<RefreshTokenResponse>>();
        Assert.NotEqual(oldRefresh, refreshed!.Data!.RefreshToken);

        // Reusing the rotated token revokes the family -> 401
        var reuse = await client.PostAsJsonAsync("/api/v1/auth/refresh", new { refreshToken = oldRefresh });
        Assert.Equal(HttpStatusCode.Unauthorized, reuse.StatusCode);
    }

    [Fact]
    public async Task Logout_RevokesRefreshToken()
    {
        var client = NewClient();
        var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = "admin@vahanx.com", password = "VahanX@Dev123!" });
        var body = await login.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

        var logout = await client.PostAsJsonAsync("/api/v1/auth/logout", new { refreshToken = body!.Data!.RefreshToken });
        Assert.Equal(HttpStatusCode.OK, logout.StatusCode);

        var refreshResponse = await client.PostAsJsonAsync("/api/v1/auth/refresh", new { refreshToken = body.Data.RefreshToken });
        Assert.Equal(HttpStatusCode.Unauthorized, refreshResponse.StatusCode);
    }

    [Fact]
    public async Task AdminUsers_WithoutToken_Returns401()
    {
        var client = NewClient();
        var response = await client.GetAsync("/api/v1/admin/users");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AdminUsers_WithSuperAdminToken_Returns200()
    {
        var client = NewClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/v1/admin/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UserWithoutPermission_Gets403()
    {
        string viewerEmail = "noperm@vahanx.com";
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
            if (!context.Users.Any(u => u.Email == viewerEmail))
            {
                var hasher = new PasswordHasher<User>();
                var user = new User { Email = viewerEmail, FirstName = "No", LastName = "Perm", PhoneNumber = "1", IsActive = true, EmailConfirmed = true };
                user.PasswordHash = hasher.HashPassword(user, "Password@123");
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }

        var client = NewClient();
        var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = viewerEmail, password = "Password@123" });
        var body = await login.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body!.Data!.AccessToken);

        var response = await client.GetAsync("/api/v1/admin/users");
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UserWithSinglePermission_Gets403OnOtherEndpoints()
    {
        string auditorEmail = "auditor@vahanx.com";
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
            var permission = context.Permissions.First(p => p.Name == AdminPermissionsAlias.AuditView);
            var role = context.Roles.FirstOrDefault(r => r.Name == "Auditor");
            if (role is null)
            {
                role = new Role { Name = "Auditor", Description = "Audit viewer", IsActive = true };
                context.Roles.Add(role);
                await context.SaveChangesAsync();
                context.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permission.Id });
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any(u => u.Email == auditorEmail))
            {
                var hasher = new PasswordHasher<User>();
                var user = new User { Email = auditorEmail, FirstName = "Audit", LastName = "Or", PhoneNumber = "1", IsActive = true, EmailConfirmed = true };
                user.PasswordHash = hasher.HashPassword(user, "Password@123");
                context.Users.Add(user);
                await context.SaveChangesAsync();
                context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id });
                await context.SaveChangesAsync();
            }
        }

        var client = NewClient();
        var login = await client.PostAsJsonAsync("/api/v1/auth/login", new { email = auditorEmail, password = "Password@123" });
        var body = await login.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body!.Data!.AccessToken);

        var auditResponse = await client.GetAsync("/api/v1/admin/audit");
        Assert.Equal(HttpStatusCode.OK, auditResponse.StatusCode);

        var usersResponse = await client.GetAsync("/api/v1/admin/users");
        Assert.Equal(HttpStatusCode.Forbidden, usersResponse.StatusCode);
    }

    [Fact]
    public async Task SuperAdminRole_CannotBeDeletedOrModified()
    {
        var client = NewClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        Guid roleId;
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
            roleId = context.Roles.First(r => r.Name == "SuperAdmin").Id;
        }

        var deleteResponse = await client.DeleteAsync($"/api/v1/admin/roles/{roleId}");
        Assert.Equal(HttpStatusCode.Forbidden, deleteResponse.StatusCode);

        var updateResponse = await client.PutAsJsonAsync($"/api/v1/admin/roles/{roleId}", new { name = "Hacked", description = "x", isActive = true });
        Assert.Equal(HttpStatusCode.Forbidden, updateResponse.StatusCode);
    }

    [Fact]
    public async Task DuplicateRoleAssignment_Returns409()
    {
        var client = NewClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        Guid userId, roleId;
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
            userId = context.Users.First(u => u.Email == "admin@vahanx.com").Id;
            roleId = context.Roles.First(r => r.Name == "SuperAdmin").Id;
        }

        var response = await client.PostAsJsonAsync($"/api/v1/admin/users/{userId}/roles", new { roleId });
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task DuplicatePermissionAssignment_Returns409()
    {
        var client = NewClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        Guid roleId, permissionId;
        using (var scope = _factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
            var role = new Role { Name = "TempRole_" + Guid.NewGuid().ToString("N")[..8], IsActive = true };
            context.Roles.Add(role);
            await context.SaveChangesAsync();
            roleId = role.Id;
            permissionId = context.Permissions.First().Id;
            context.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
            await context.SaveChangesAsync();
        }

        var response = await client.PostAsJsonAsync($"/api/v1/admin/roles/{roleId}/permissions", new { permissionId });
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task LoginEvent_IsRecordedInLoginHistory()
    {
        var client = NewClient();
        await client.PostAsJsonAsync("/api/v1/auth/login", new { email = "admin@vahanx.com", password = "VahanX@Dev123!" });

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
        Assert.True(context.LoginHistories.Any(h => h.Email == "admin@vahanx.com" && h.EventType == VahanX.Domain.Enums.LoginEventType.Login));
    }

    [Fact]
    public async Task RoleCreate_IsRecordedInAuditLog()
    {
        var client = NewClient();
        var token = await LoginAsync(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await client.PostAsJsonAsync("/api/v1/admin/roles", new { name = "AuditTestRole_" + Guid.NewGuid().ToString("N")[..8], description = "test", isActive = true });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<VahanXDbContext>();
        Assert.True(context.AuditLogs.Any(a => a.EntityType == "Role" && a.Action == VahanX.Domain.Enums.AuditAction.Create));
    }

    private static class AdminPermissionsAlias
    {
        public const string AuditView = "AUDIT_VIEW";
    }
}
