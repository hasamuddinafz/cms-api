using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;

namespace NexusCms.Application.PageModels.Queries.ExportPageModels;

public class ExportPageModelsQueryHandler(IDbContextAccessor db, IExportService exportService)
    : IRequestHandler<ExportPageModelsQuery, ExportResult>
{
    public async Task<ExportResult> Handle(ExportPageModelsQuery request, CancellationToken ct)
    {
        var q = db.PageModels.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Request.Search))
        {
            var term = request.Request.Search.Trim().ToLower();
            q = q.Where(m => m.Name.ToLower().Contains(term) || m.Key.ToLower().Contains(term));
        }

        if (request.Request.Filters.TryGetValue("isSingleton", out var singleton))
        {
            var val = bool.Parse(singleton);
            q = q.Where(m => m.IsSingleton == val);
        }

        var rows = await q
            .OrderBy(m => m.Name)
            .Select(m => new PageModelExportRow(
                m.Key, m.Name, m.Description, m.IsSingleton,
                m.PageInstances.Count, m.CreatedAt))
            .ToListAsync(ct);

        var isExcel = request.Format.Equals("excel", StringComparison.OrdinalIgnoreCase);

        return isExcel
            ? new ExportResult(
                exportService.ExportExcel(rows, "Page Models"),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"page-models-{DateTime.UtcNow:yyyyMMdd}.xlsx")
            : new ExportResult(
                exportService.ExportCsv(rows),
                "text/csv",
                $"page-models-{DateTime.UtcNow:yyyyMMdd}.csv");
    }
}
