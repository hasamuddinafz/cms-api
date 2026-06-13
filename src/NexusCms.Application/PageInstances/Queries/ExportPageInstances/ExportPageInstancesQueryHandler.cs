using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.PageModels.Queries.ExportPageModels;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.PageInstances.Queries.ExportPageInstances;

public class ExportPageInstancesQueryHandler(IDbContextAccessor db, IExportService exportService)
    : IRequestHandler<ExportPageInstancesQuery, ExportResult>
{
    public async Task<ExportResult> Handle(ExportPageInstancesQuery request, CancellationToken ct)
    {
        var q = db.PageInstances.AsQueryable();

        if (request.PageModelId.HasValue)
            q = q.Where(i => i.PageModelId == request.PageModelId.Value);

        if (request.Request.Filters.TryGetValue("modelKey", out var mk) && !string.IsNullOrWhiteSpace(mk))
            q = q.Where(i => i.PageModel.Key == mk);

        if (request.Request.Filters.TryGetValue("status", out var statusStr)
            && Enum.TryParse<PageStatus>(statusStr, true, out var status))
            q = q.Where(i => i.Status == status);

        if (!string.IsNullOrWhiteSpace(request.Request.Search))
        {
            var term = request.Request.Search.Trim().ToLower();
            q = q.Where(i => i.Slug.ToLower().Contains(term) || i.PageModel.Name.ToLower().Contains(term));
        }

        var rows = await q
            .OrderByDescending(i => i.UpdatedAt)
            .Select(i => new PageInstanceExportRow(
                i.PageModel.Key,
                i.Slug,
                i.Status.ToString(),
                i.ViewCount,
                i.PublishedAt.HasValue ? i.PublishedAt.Value.ToString("yyyy-MM-dd HH:mm") : null,
                i.CreatedAt.ToString("yyyy-MM-dd HH:mm")))
            .ToListAsync(ct);

        var isExcel = request.Format.Equals("excel", StringComparison.OrdinalIgnoreCase);
        return isExcel
            ? new ExportResult(
                exportService.ExportExcel(rows, "Sayfa İçerikleri"),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"pages-{DateTime.UtcNow:yyyyMMdd}.xlsx")
            : new ExportResult(
                exportService.ExportCsv(rows),
                "text/csv",
                $"pages-{DateTime.UtcNow:yyyyMMdd}.csv");
    }
}
