using NexusCms.Domain.Common;

namespace NexusCms.Domain.Entities;

/// <summary>
/// Singleton table — always exactly one row (Id = well-known constant).
/// All sub-sections are JSONB columns for flexible extension without schema migrations.
/// SMTP password inside SmtpJson must be stored AES-256 encrypted.
/// </summary>
public class AppSetting : Entity
{
    public static readonly Guid SingletonId = new("00000000-0000-0000-0000-000000000001");

    public string GeneralJson { get; set; } = "{}";        // site name, logo, favicon, contact, social links
    public string SeoJson { get; set; } = "{}";            // default title template, meta, OG image, analytics
    public string SmtpJson { get; set; } = "{}";           // host, port, credentials (password AES-encrypted)
    public string LocalizationJson { get; set; } = "{}";   // active languages, default language, date format
    public string ThemeJson { get; set; } = "{}";          // primary/secondary colors, font, etc.

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
