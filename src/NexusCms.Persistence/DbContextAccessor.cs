using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence;

public class DbContextAccessor(AppDbContext context) : IDbContextAccessor
{
    public DbSet<RefreshToken> RefreshTokens    => context.RefreshTokens;
    public DbSet<Permission>   Permissions       => context.Permissions;
    public DbSet<Role>         Roles             => context.Roles;
    public DbSet<RolePermission> RolePermissions => context.RolePermissions;
    public DbSet<UserRole>       UserRoles       => context.UserRoles;
    public DbSet<PageModel>    PageModels        => context.PageModels;
    public DbSet<PageInstance> PageInstances     => context.PageInstances;
    public DbSet<FileAsset>    FileAssets        => context.FileAssets;
    public DbSet<Language>     Languages         => context.Languages;
    public DbSet<Menu>         Menus             => context.Menus;
    public DbSet<AppSetting>   AppSettings       => context.AppSettings;
    public DbSet<AuditLog>        AuditLogs       => context.AuditLogs;
    public DbSet<PageViewLog>     PageViewLogs    => context.PageViewLogs;
    public DbSet<ApplicationUser> Users           => context.Users;

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);

    public async Task<IReadOnlyList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default)
    {
        // Resolve roles for user → resolve permissions for each role
        var permissions = await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(context.RolePermissions, ur => ur.RoleId, rp => rp.RoleId, (_, rp) => rp.PermissionId)
            .Join(context.Permissions, pid => pid, p => p.Id, (_, p) => p.Key)
            .Distinct()
            .ToListAsync(ct);

        return permissions;
    }
}
