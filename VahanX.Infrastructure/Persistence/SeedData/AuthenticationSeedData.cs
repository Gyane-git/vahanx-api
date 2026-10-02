using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VahanX.Application.DTOs.Admin;
using VahanX.Domain.Entities;

namespace VahanX.Infrastructure.Persistence.SeedData;

/// <summary>
/// Seeds system roles, permissions, role-permission assignments and a development super admin.
/// Development credentials come from configuration ("Seed" section). Never hardcode production credentials.
/// </summary>
public static class AuthenticationSeedData
{
    public static async Task SeedAsync(VahanXDbContext context, IConfiguration configuration, CancellationToken cancellationToken = default)
    {
        // Ensure schema exists/up-to-date (relational providers only)
        if (context.Database.ProviderName?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) == true)
            await context.Database.MigrateAsync(cancellationToken);

        // 1. Permissions
        var permissionNames = typeof(AdminPermissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Select(f => (string)f.GetValue(null)!)
            .Distinct()
            .ToList();

        var existingPermissions = await context.Permissions.ToListAsync(cancellationToken);
        foreach (var name in permissionNames)
        {
            if (existingPermissions.Any(p => p.Name == name))
                continue;

            context.Permissions.Add(new Permission
            {
                Name = name,
                Description = name.Replace('_', ' ').ToLowerInvariant(),
                Module = name.Split('_')[0],
                IsSystemDefined = true
            });
        }
        await context.SaveChangesAsync(cancellationToken);

        // 2. SUPER_ADMIN role
        var superAdmin = await context.Roles.FirstOrDefaultAsync(r => r.Name == AdminRoles.SuperAdmin, cancellationToken);
        if (superAdmin is null)
        {
            superAdmin = new Role
            {
                Name = AdminRoles.SuperAdmin,
                Description = "Full access to all VahanX platform administration features.",
                IsSystemRole = true,
                IsActive = true
            };
            context.Roles.Add(superAdmin);
            await context.SaveChangesAsync(cancellationToken);
        }

        // 3. All permissions -> SUPER_ADMIN
        var allPermissions = await context.Permissions.ToListAsync(cancellationToken);
        var existingLinks = await context.RolePermissions.Where(rp => rp.RoleId == superAdmin.Id).ToListAsync(cancellationToken);
        foreach (var permission in allPermissions)
        {
            if (existingLinks.Any(l => l.PermissionId == permission.Id))
                continue;
            context.RolePermissions.Add(new RolePermission { RoleId = superAdmin.Id, PermissionId = permission.Id });
        }
        await context.SaveChangesAsync(cancellationToken);

        // 4. Development super admin user
        var email = configuration["Seed:AdminEmail"] ?? "admin@vahanx.com";
        var password = configuration["Seed:AdminPassword"];
        if (string.IsNullOrWhiteSpace(password))
            return; // No seed user without explicitly configured credentials.

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user is null)
        {
            var hasher = new PasswordHasher<User>();
            user = new User
            {
                Email = email,
                PhoneNumber = "+977-9800000000",
                FirstName = "Super",
                LastName = "Admin",
                IsActive = true,
                EmailConfirmed = true
            };
            user.PasswordHash = hasher.HashPassword(user, password);
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
        }

        var hasRole = await context.UserRoles.AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == superAdmin.Id, cancellationToken);
        if (!hasRole)
        {
            context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = superAdmin.Id });
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
