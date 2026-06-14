using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Dashboard.Queries.GetViewTrend;

public class GetViewTrendQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetViewTrendQuery, Result<List<DailyViewDto>>>
{
    public async Task<Result<List<DailyViewDto>>> Handle(GetViewTrendQuery request, CancellationToken ct)
    {
        var since = DateTime.UtcNow.Date.AddDays(-request.Days + 1);

        var raw = await db.PageViewLogs
            .AsNoTracking()
            .Where(l => l.ViewedAt >= since)
            .GroupBy(l => l.ViewedAt.Date)
            .Select(g => new { Date = g.Key, Views = (long)g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync(ct);

        // Fill gaps so every day in the range appears
        var result = Enumerable.Range(0, request.Days)
            .Select(i =>
            {
                var d    = since.AddDays(i);
                var item = raw.FirstOrDefault(r => r.Date == d);
                return new DailyViewDto(d.ToString("yyyy-MM-dd"), item?.Views ?? 0);
            })
            .ToList();

        return Result<List<DailyViewDto>>.Success(result);
    }
}
