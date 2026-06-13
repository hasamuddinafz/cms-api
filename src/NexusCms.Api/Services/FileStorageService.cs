using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Options;
using NexusCms.Domain.Enums;
using SkiaSharp;

namespace NexusCms.Api.Services;

public partial class FileStorageService(
    IWebHostEnvironment env,
    IOptions<FileStorageOptions> options) : IFileStorageService
{
    private readonly FileStorageOptions _opts = options.Value;

    public async Task<StoredFileResult> StoreAsync(
        Stream content,
        string originalFileName,
        string mimeType,
        string? folder = null,
        CancellationToken ct = default)
    {
        var fileType = ResolveFileType(mimeType);
        var ext      = Path.GetExtension(originalFileName).ToLowerInvariant();
        var slug     = Slugify(Path.GetFileNameWithoutExtension(originalFileName));
        var guid     = Guid.NewGuid().ToString("N")[..12];
        var fileName = $"{guid}-{slug}{ext}";

        var typeDir  = fileType.ToString().ToLower() + "s";   // images / documents / videos / others
        var datePath = Path.Combine(DateTime.UtcNow.Year.ToString(), DateTime.UtcNow.Month.ToString("D2"));
        var relDir   = Path.Combine("uploads", typeDir, datePath);
        if (!string.IsNullOrWhiteSpace(folder))
            relDir = Path.Combine(relDir, Slugify(folder));

        var absDir  = Path.Combine(env.WebRootPath, relDir);
        Directory.CreateDirectory(absDir);

        // Buffer to memory — needed to re-read for thumbnail without seeking the original stream
        using var ms = new MemoryStream();
        await content.CopyToAsync(ms, ct);
        var sizeBytes = ms.Length;
        ms.Position   = 0;

        var absPath = Path.Combine(absDir, fileName);
        await using (var fs = File.Create(absPath))
            await ms.CopyToAsync(fs, ct);

        var url          = $"/{relDir.Replace('\\', '/')}/{fileName}";
        string? thumbUrl = null;

        if (fileType == FileType.Image)
        {
            ms.Position = 0;
            thumbUrl    = await Task.Run(() => CreateThumbnail(ms, absDir, guid, slug, ext, relDir), ct);
        }

        return new StoredFileResult(fileName, url, thumbUrl, fileType, sizeBytes);
    }

    public Task DeleteAsync(string url, string? thumbnailUrl, CancellationToken ct = default)
    {
        DeletePhysical(url);
        if (thumbnailUrl is not null) DeletePhysical(thumbnailUrl);
        return Task.CompletedTask;
    }

    // ── Helpers ──────────────────────────────────────────────────────────────

    private string? CreateThumbnail(Stream src, string absDir, string guid, string slug, string ext, string relDir)
    {
        try
        {
            using var skData   = SKData.Create(src);
            using var original = SKBitmap.Decode(skData);
            if (original is null) return null;

            var (tw, th) = FitSize(original.Width, original.Height, _opts.ThumbnailWidth, _opts.ThumbnailHeight);
            using var thumb = original.Resize(new SKImageInfo(tw, th), SKSamplingOptions.Default);
            if (thumb is null) return null;

            using var image  = SKImage.FromBitmap(thumb);
            using var data   = image.Encode(SKEncodedImageFormat.Jpeg, 85);

            var thumbName = $"{guid}-{slug}_thumb.jpg";
            var thumbPath = Path.Combine(absDir, thumbName);
            using var fs  = File.Create(thumbPath);
            data.SaveTo(fs);

            return $"/{relDir.Replace('\\', '/')}/{thumbName}";
        }
        catch
        {
            return null;  // Non-fatal: thumbnail failure doesn't abort the upload
        }
    }

    private static (int w, int h) FitSize(int srcW, int srcH, int maxW, int maxH)
    {
        if (srcW <= maxW && srcH <= maxH) return (srcW, srcH);
        var ratio = Math.Min((double)maxW / srcW, (double)maxH / srcH);
        return ((int)(srcW * ratio), (int)(srcH * ratio));
    }

    private void DeletePhysical(string relativeUrl)
    {
        var abs = Path.Combine(
            env.WebRootPath,
            relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(abs)) File.Delete(abs);
    }

    internal static FileType ResolveFileType(string mimeType) => mimeType switch
    {
        var m when m.StartsWith("image/")             => FileType.Image,
        var m when m.StartsWith("video/")             => FileType.Video,
        "application/pdf"                             => FileType.Document,
        "application/msword"                          => FileType.Document,
        var m when m.StartsWith("application/vnd.")   => FileType.Document,
        "text/csv"                                    => FileType.Document,
        _                                             => FileType.Other
    };

    private static string Slugify(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return "file";
        var s = input.ToLowerInvariant();
        s = NonAlphanumericRegex().Replace(s, "-");
        s = MultiDashRegex().Replace(s, "-").Trim('-');
        return s.Length > 80 ? s[..80] : s;
    }

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex NonAlphanumericRegex();

    [GeneratedRegex(@"-{2,}")]
    private static partial Regex MultiDashRegex();
}
