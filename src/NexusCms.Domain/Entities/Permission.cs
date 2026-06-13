using NexusCms.Domain.Common;

namespace NexusCms.Domain.Entities;

/// <summary>
/// Seed-only entity — the full list is defined in code (PermissionSeed).
/// Key format: "{module}.{action}" e.g. "pages.publish", "media.delete".
/// </summary>
public class Permission : Entity
{
    public string Key { get; set; } = string.Empty;        // e.g. "pages.edit"
    public string Module { get; set; } = string.Empty;     // e.g. "Pages"
    public string Label { get; set; } = string.Empty;      // human-readable panel label

    public ICollection<RolePermission> RolePermissions { get; set; } = [];
}
