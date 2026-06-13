using NexusCms.Domain.Common;

namespace NexusCms.Domain.Entities;

/// <summary>
/// Defines the schema (sections + fields) for a page type.
/// SchemaJson is stored as JSONB; IsSingleton means only one PageInstance is allowed.
/// </summary>
public class PageModel : AuditableEntity
{
    public string Key { get; set; } = string.Empty;        // e.g. "home", "service-detail"
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }                      // Lucide icon name
    public bool IsSingleton { get; set; }
    public string SchemaJson { get; set; } = "{}";         // JSONB

    public ICollection<PageInstance> PageInstances { get; set; } = [];
}
