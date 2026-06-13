using Microsoft.AspNetCore.Identity;

namespace NexusCms.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }

    /// <summary>
    /// System roles (e.g. SuperAdmin) cannot be deleted or renamed.
    /// </summary>
    public bool IsSystemRole { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
