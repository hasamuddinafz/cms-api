using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Domain.Entities;

/// <summary>
/// Represents any uploaded file (image, document, video, other).
/// Physical path follows: wwwroot/uploads/{fileType}/{yyyy}/{MM}/{guid}-{name}.ext
/// ThumbnailUrl is populated only for Image type.
/// AltTextJson is JSONB keyed by language code — only meaningful for images.
/// </summary>
public class FileAsset : Entity
{
    public string FileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public FileType FileType { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string AltTextJson { get; set; } = "{}";        // JSONB — { "tr": "...", "en": "..." }
    public string? Folder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedByUserId { get; set; }
}
