using NexusCms.Domain.Enums;

namespace NexusCms.Application.Common.Options;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";

    public List<string> AllowedMimeTypes { get; init; } = [];

    /// <summary>Max sizes in bytes, keyed by FileType enum name.</summary>
    public Dictionary<string, long> MaxFileSizeBytes { get; init; } = new()
    {
        [nameof(FileType.Image)]    = 5 * 1024 * 1024,
        [nameof(FileType.Document)] = 20 * 1024 * 1024,
        [nameof(FileType.Video)]    = 100 * 1024 * 1024,
        [nameof(FileType.Other)]    = 10 * 1024 * 1024
    };

    public int ThumbnailWidth  { get; init; } = 400;
    public int ThumbnailHeight { get; init; } = 300;

    public long MaxFileSizeFor(FileType type)
        => MaxFileSizeBytes.TryGetValue(type.ToString(), out var size) ? size : 10 * 1024 * 1024;
}
