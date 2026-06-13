using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Domain.Entities;

public class AuditLog : Entity
{
    public Guid? UserId { get; set; }
    public AuditAction Action { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string? EntityId { get; set; }

    /// <summary>
    /// JSONB — { "before": {...}, "after": {...} } diff of changed fields.
    /// </summary>
    public string? ChangesJson { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
}
