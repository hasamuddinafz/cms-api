using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Menus;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence;

/// <summary>
/// Idempotent seed: runs on every app startup, safe to call multiple times.
/// Adds missing permissions, creates SuperAdmin role if absent, creates the
/// default SuperAdmin user if absent.
/// </summary>
public static class DatabaseSeeder
{
    public const string SuperAdminRoleName = "SuperAdmin";
    private const string SuperAdminDescription = "System role with all permissions.";
    private static readonly Guid SuperAdminRoleId = new("A0000000-0000-0000-0000-000000000001");

    public static async Task SeedAsync(
        AppDbContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<Role> roleManager)
    {
        await SeedPermissionsAsync(db);
        await SeedSuperAdminRoleAsync(db, roleManager);
        await SeedSuperAdminUserAsync(userManager);
        await SeedMenusAsync(db);
    }

    private static async Task SeedPermissionsAsync(AppDbContext db)
    {
        var existingKeys = await db.Permissions
            .Select(p => p.Key)
            .ToHashSetAsync();

        var toAdd = Permissions.All()
            .Where(p => !existingKeys.Contains(p.Key))
            .Select(p => new Permission
            {
                Id     = p.Id,
                Key    = p.Key,
                Module = p.Module,
                Label  = p.Label
            })
            .ToList();

        if (toAdd.Count > 0)
        {
            await db.Permissions.AddRangeAsync(toAdd);
            await db.SaveChangesAsync();
        }

        // Keep labels/modules of existing permissions in sync with the code definitions
        var definitions = Permissions.All().ToDictionary(p => p.Key);
        var existing = await db.Permissions.ToListAsync();
        var changed = false;
        foreach (var perm in existing)
        {
            if (!definitions.TryGetValue(perm.Key, out var def)) continue;
            if (perm.Label != def.Label || perm.Module != def.Module)
            {
                perm.Label  = def.Label;
                perm.Module = def.Module;
                changed = true;
            }
        }
        if (changed) await db.SaveChangesAsync();

        // Grant any newly added permissions to SuperAdmin role
        var superAdminRole = await db.Roles.FindAsync(SuperAdminRoleId);
        if (superAdminRole is null) return;

        var grantedIds = await db.RolePermissions
            .Where(rp => rp.RoleId == SuperAdminRoleId)
            .Select(rp => rp.PermissionId)
            .ToHashSetAsync();

        var allPermissionIds = await db.Permissions.Select(p => p.Id).ToListAsync();

        var missingGrants = allPermissionIds
            .Where(pid => !grantedIds.Contains(pid))
            .Select(pid => new RolePermission { RoleId = SuperAdminRoleId, PermissionId = pid })
            .ToList();

        if (missingGrants.Count > 0)
        {
            await db.RolePermissions.AddRangeAsync(missingGrants);
            await db.SaveChangesAsync();
        }
    }

    private static async Task SeedSuperAdminRoleAsync(AppDbContext db, RoleManager<Role> roleManager)
    {
        var existingRole = await roleManager.FindByNameAsync(SuperAdminRoleName);
        if (existingRole is not null)
        {
            if (existingRole.Description != SuperAdminDescription)
            {
                existingRole.Description = SuperAdminDescription;
                await roleManager.UpdateAsync(existingRole);
            }
            return;
        }

        var role = new Role
        {
            Id           = SuperAdminRoleId,
            Name         = SuperAdminRoleName,
            NormalizedName = SuperAdminRoleName.ToUpperInvariant(),
            Description  = SuperAdminDescription,
            IsSystemRole = true
        };

        await roleManager.CreateAsync(role);

        // Grant all permissions immediately
        var allPermissionIds = await db.Permissions.Select(p => p.Id).ToListAsync();
        var grants = allPermissionIds
            .Select(pid => new RolePermission { RoleId = role.Id, PermissionId = pid })
            .ToList();

        await db.RolePermissions.AddRangeAsync(grants);
        await db.SaveChangesAsync();
    }

    private static async Task SeedSuperAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        const string email    = "admin@nexuscms.local";
        const string password = "Admin1234!";

        if (await userManager.FindByEmailAsync(email) is not null) return;

        var user = new ApplicationUser
        {
            UserName       = email,
            Email          = email,
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = email.ToUpperInvariant(),
            FullName       = "Super Admin",
            IsActive       = true,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, SuperAdminRoleName);
    }

    private static async Task SeedMenusAsync(AppDbContext db)
    {
        var existingKeys = await db.Menus.Select(m => m.Key).ToHashSetAsync();

        var missing = MenuKeys.All
            .Where(m => !existingKeys.Contains(m.Key))
            .Select(m => new Menu { Key = m.Key, Type = m.Type, ItemsJson = "[]" })
            .ToList();

        if (missing.Count > 0)
        {
            await db.Menus.AddRangeAsync(missing);
            await db.SaveChangesAsync();
        }
    }
}
