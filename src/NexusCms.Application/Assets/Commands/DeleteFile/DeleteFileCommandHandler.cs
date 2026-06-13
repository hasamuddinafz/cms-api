using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Assets.Commands.DeleteFile;

public class DeleteFileCommandHandler(IDbContextAccessor db, IFileStorageService storage)
    : IRequestHandler<DeleteFileCommand, Result>
{
    public async Task<Result> Handle(DeleteFileCommand request, CancellationToken ct)
    {
        var asset = await db.FileAssets.FindAsync([request.Id], ct);
        if (asset is null)
            return Result.Failure("Dosya bulunamadı.");

        await storage.DeleteAsync(asset.Url, asset.ThumbnailUrl, ct);
        db.FileAssets.Remove(asset);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}
