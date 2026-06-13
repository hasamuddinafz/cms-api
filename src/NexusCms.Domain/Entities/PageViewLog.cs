using NexusCms.Domain.Common;

namespace NexusCms.Domain.Entities;

/// <summary>
/// Detailed per-hit log for analytics. PageInstance.ViewCount is the denormalised counter;
/// this table powers "top pages", trend charts, and country breakdowns.
/// IpHash is stored instead of raw IP for privacy.
/// </summary>
public class PageViewLog : Entity
{
    public Guid PageInstanceId { get; set; }
    public PageInstance PageInstance { get; set; } = null!;

    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    public string? IpHash { get; set; }
    public string? UserAgent { get; set; }
    public string? Referrer { get; set; }
    public string? CountryCode { get; set; }
}
