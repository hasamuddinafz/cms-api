using NexusCms.Domain.Enums;

namespace NexusCms.Application.Assets;

public record FileAssetDto(
    Guid Id,
    string FileName,
    string OriginalFileName,
    string MimeType,
    long SizeBytes,
    FileType FileType,
    string Url,
    string? ThumbnailUrl,
    string AltTextJson,
    string? Folder,
    DateTime CreatedAt,
    Guid? CreatedByUserId
);

public record FileAssetExportRow(
    string OriginalFileName,
    string FileType,
    string MimeType,
    long SizeBytes,
    string? Folder,
    string Url,
    string CreatedAt
);
