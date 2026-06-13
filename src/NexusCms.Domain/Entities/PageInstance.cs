using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Domain.Entities;

/// <summary>
/// Holds the actual content for a page.
/// ContentJson is JSONB keyed by language code, then section key.
/// SeoMetaJson is JSONB keyed by language code.
/// </summary>
public class PageInstance : AuditableEntity
{
    public Guid PageModelId { get; set; }
    public PageModel PageModel { get; set; } = null!;

    public string Slug { get; set; } = string.Empty;
    public PageStatus Status { get; set; } = PageStatus.Draft;

    public string ContentJson { get; set; } = "{}";        // JSONB — { "tr": { "hero": {...} }, "en": {...} }
    public string SeoMetaJson { get; set; } = "{}";        // JSONB — { "tr": { "title": "...", ... } }

    public long ViewCount { get; set; }
    public DateTime? PublishedAt { get; set; }

    public Guid? CreatedByUserId { get; set; }
    public Guid? UpdatedByUserId { get; set; }

    public ICollection<PageViewLog> ViewLogs { get; set; } = [];
}
