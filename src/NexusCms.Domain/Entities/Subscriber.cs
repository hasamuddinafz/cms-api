using NexusCms.Domain.Common;

namespace NexusCms.Domain.Entities;

public class Subscriber : Entity
{
    public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public bool IsActive { get; set; } = true;
    public string? IpAddress { get; set; }
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UnsubscribedAt { get; set; }
}
