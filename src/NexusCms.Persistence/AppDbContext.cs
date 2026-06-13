using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexusCms.Domain.Entities;

namespace NexusCms.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, Role, Guid,
        Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>,
        UserRole,
        Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>,
        Microsoft.AspNetCore.Identity.IdentityRoleClaim<Guid>,
        Microsoft.AspNetCore.Identity.IdentityUserToken<Guid>>(options)
{
    public DbSet<PageModel> PageModels => Set<PageModel>();
    public DbSet<PageInstance> PageInstances => Set<PageInstance>();
    public DbSet<FileAsset> FileAssets => Set<FileAsset>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<AppSetting> AppSettings => Set<AppSetting>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<PageViewLog> PageViewLogs => Set<PageViewLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Domain.Common.AuditableEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
