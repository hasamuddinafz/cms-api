using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.PageModels.Queries.ExportPageModels;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Assets.Queries.ExportFiles;

public class ExportFilesQueryHandler(IDbContextAccessor db, IExportService exportService)
    : IRequestHandler<ExportFilesQuery, ExportResult>
{
    public async Task<ExportResult> Handle(ExportFilesQuery request, CancellationToken ct)
    {
        var q = db.FileAssets.AsQueryable();

        if (request.Request.Filters.TryGetValue("fileType", out var ftStr)
            && Enum.TryParse<FileType>(ftStr, true, out var ft))
            q = q.Where(a => a.FileType == ft);

        if (request.Request.Filters.TryGetValue("folder", out var folder) && !string.IsNullOrWhiteSpace(folder))
            q = q.Where(a => a.Folder == folder);

        if (!string.IsNullOrWhiteSpace(request.Request.Search))
        {
            var term = request.Request.Search.Trim().ToLower();
            q = q.Where(a => a.OriginalFileName.ToLower().Contains(term));
        }

        var rows = await q
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new FileAssetExportRow(
                a.OriginalFileName,
                a.FileType.ToString(),
                a.MimeType,
                a.SizeBytes,
                a.Folder,
                a.Url,
                a.CreatedAt.ToString("yyyy-MM-dd HH:mm")))
            .ToListAsync(ct);

        var isExcel = request.Format.Equals("excel", StringComparison.OrdinalIgnoreCase);
        return isExcel
            ? new ExportResult(
                exportService.ExportExcel(rows, "Medya"),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"media-{DateTime.UtcNow:yyyyMMdd}.xlsx")
            : new ExportResult(
                exportService.ExportCsv(rows),
                "text/csv",
                $"media-{DateTime.UtcNow:yyyyMMdd}.csv");
    }
}
