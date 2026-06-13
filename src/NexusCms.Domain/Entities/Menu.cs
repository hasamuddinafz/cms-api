using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Domain.Entities;

/// <summary>
/// ItemsJson is JSONB containing a hierarchical array of MenuItems with multilingual labels.
/// Separate records exist per key: "navbar-desktop", "navbar-mobile", "footer-desktop", "footer-mobile".
/// </summary>
public class Menu : AuditableEntity
{
    public string Key { get; set; } = string.Empty;       // e.g. "navbar-desktop"
    public MenuType Type { get; set; }
    public string ItemsJson { get; set; } = "[]";          // JSONB
}
