using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Assets.Queries.GetFileById;

public class GetFileByIdQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetFileByIdQuery, Result<FileAssetDto>>
{
    public async Task<Result<FileAssetDto>> Handle(GetFileByIdQuery request, CancellationToken ct)
    {
        var a = await db.FileAssets.AsNoTracking().FirstOrDefaultAsync(f => f.Id == request.Id, ct);
        return a is not null
            ? Result.Success(new FileAssetDto(
                a.Id, a.FileName, a.OriginalFileName, a.MimeType,
                a.SizeBytes, a.FileType, a.Url, a.ThumbnailUrl,
                a.AltTextJson, a.Folder, a.CreatedAt, a.CreatedByUserId))
            : Result.Failure<FileAssetDto>("File not found.");
    }
}
