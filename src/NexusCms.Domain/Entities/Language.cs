using NexusCms.Domain.Common;

namespace NexusCms.Domain.Entities;

public class Language : Entity
{
    public string Code { get; set; } = string.Empty;      // e.g. "en", "de"
    public string Name { get; set; } = string.Empty;      // e.g. "English", "Deutsch"
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; } = true;
    public string? FlagIcon { get; set; }
    public int SortOrder { get; set; }
}
