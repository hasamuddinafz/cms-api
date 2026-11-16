using MediatR;
using Microsoft.Extensions.Options;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Options;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Assets.Commands.UploadFile;

public class UploadFileCommandHandler(
    IFileStorageService storage,
    IDbContextAccessor db,
    ICurrentUserService currentUser,
    IOptions<FileStorageOptions> options)
    : IRequestHandler<UploadFileCommand, Result<FileAssetDto>>
{
    public async Task<Result<FileAssetDto>> Handle(UploadFileCommand request, CancellationToken ct)
    {
        var opts     = options.Value;
        var stored   = await storage.StoreAsync(request.Content, request.OriginalFileName, request.MimeType, request.Folder, ct);

        // Size limit check (done after type resolution so we know the FileType)
        var limit = opts.MaxFileSizeFor(stored.FileType);
        if (request.SizeBytes > limit)
        {
            await storage.DeleteAsync(stored.Url, stored.ThumbnailUrl, ct);
            return Result.Failure<FileAssetDto>($"File size limit exceeded ({limit / 1024 / 1024} MB).");
        }

        var asset = new FileAsset
        {
            FileName         = stored.FileName,
            OriginalFileName = request.OriginalFileName,
            MimeType         = request.MimeType,
            SizeBytes        = stored.SizeBytes,
            FileType         = stored.FileType,
            Url              = stored.Url,
            ThumbnailUrl     = stored.ThumbnailUrl,
            AltTextJson      = "{}",
            Folder           = request.Folder,
            CreatedByUserId  = currentUser.UserId
        };

        await db.FileAssets.AddAsync(asset, ct);
        await db.SaveChangesAsync(ct);

        return Result.Success(ToDto(asset));
    }

    private static FileAssetDto ToDto(FileAsset a) => new(
        a.Id, a.FileName, a.OriginalFileName, a.MimeType,
        a.SizeBytes, a.FileType, a.Url, a.ThumbnailUrl,
        a.AltTextJson, a.Folder, a.CreatedAt, a.CreatedByUserId);
}
