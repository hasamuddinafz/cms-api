using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Assets.Queries.GetFileList;

public class GetFileListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetFileListQuery, PagedResult<FileAssetDto>>
{
    public async Task<PagedResult<FileAssetDto>> Handle(GetFileListQuery request, CancellationToken ct)
    {
        var q = db.FileAssets.AsQueryable();

        // fileType filter
        if (request.Request.Filters.TryGetValue("fileType", out var ftStr)
            && Enum.TryParse<FileType>(ftStr, true, out var ft))
            q = q.Where(a => a.FileType == ft);

        // folder filter
        if (request.Request.Filters.TryGetValue("folder", out var folder) && !string.IsNullOrWhiteSpace(folder))
            q = q.Where(a => a.Folder == folder);

        // date range
        if (request.Request.Filters.TryGetValue("from", out var fromStr)
            && DateTime.TryParse(fromStr, out var from))
            q = q.Where(a => a.CreatedAt >= from);

        if (request.Request.Filters.TryGetValue("to", out var toStr)
            && DateTime.TryParse(toStr, out var to))
            q = q.Where(a => a.CreatedAt <= to);

        // search by original filename
        if (!string.IsNullOrWhiteSpace(request.Request.Search))
        {
            var term = request.Request.Search.Trim().ToLower();
            q = q.Where(a => a.OriginalFileName.ToLower().Contains(term));
        }

        q = (request.Request.SortBy?.ToLower(), request.Request.SortDir?.ToLower()) switch
        {
            ("name",       "asc")  => q.OrderBy(a => a.OriginalFileName),
            ("name",       _)      => q.OrderByDescending(a => a.OriginalFileName),
            ("size",       "asc")  => q.OrderBy(a => a.SizeBytes),
            ("size",       _)      => q.OrderByDescending(a => a.SizeBytes),
            ("createdat",  "asc")  => q.OrderBy(a => a.CreatedAt),
            _                      => q.OrderByDescending(a => a.CreatedAt)
        };

        var total = await q.CountAsync(ct);
        var items = await q
            .Skip(request.Request.Skip)
            .Take(request.Request.PageSize)
            .Select(a => new FileAssetDto(
                a.Id, a.FileName, a.OriginalFileName, a.MimeType,
                a.SizeBytes, a.FileType, a.Url, a.ThumbnailUrl,
                a.AltTextJson, a.Folder, a.CreatedAt, a.CreatedByUserId))
            .ToListAsync(ct);

        return PagedResult<FileAssetDto>.Create(items, total, request.Request);
    }
}
