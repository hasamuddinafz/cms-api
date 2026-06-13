using NexusCms.Domain.Enums;

namespace NexusCms.Application.Common.Interfaces;

public interface IFileStorageService
{
    /// <summary>
    /// Saves the stream to disk under uploads/{type}/{yyyy}/{MM}/, generates a GUID-prefixed
    /// slug filename, creates a thumbnail for images, and returns the populated paths.
    /// </summary>
    Task<StoredFileResult> StoreAsync(
        Stream content,
        string originalFileName,
        string mimeType,
        string? folder = null,
        CancellationToken ct = default);

    Task DeleteAsync(string url, string? thumbnailUrl, CancellationToken ct = default);
}

public record StoredFileResult(
    string FileName,
    string Url,
    string? ThumbnailUrl,
    FileType FileType,
    long SizeBytes
);
