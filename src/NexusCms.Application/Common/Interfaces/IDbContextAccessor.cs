using Microsoft.EntityFrameworkCore;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Common.Interfaces;

/// <summary>
/// Thin abstraction over AppDbContext exposed to Application layer.
/// Keeps Application free of direct EF/Npgsql dependencies.
/// </summary>
public interface IDbContextAccessor
{
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<Role> Roles { get; }
    DbSet<RolePermission> RolePermissions { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<PageModel> PageModels { get; }
    DbSet<PageInstance> PageInstances { get; }
    DbSet<FileAsset> FileAssets { get; }
    DbSet<Language> Languages { get; }
    DbSet<Menu> Menus { get; }
    DbSet<AppSetting> AppSettings { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<ApplicationUser> Users { get; }
    DbSet<PageViewLog> PageViewLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>Resolves all permission keys for a user via their roles.</summary>
    Task<IReadOnlyList<string>> GetUserPermissionsAsync(Guid userId, CancellationToken ct = default);
}
