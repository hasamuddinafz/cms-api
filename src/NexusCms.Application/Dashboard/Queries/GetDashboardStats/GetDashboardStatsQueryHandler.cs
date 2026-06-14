using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Dashboard.Queries.GetDashboardStats;

public class GetDashboardStatsQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
{
    public async Task<Result<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken ct)
    {
        var today = DateTime.UtcNow.Date;

        var totalPages     = await db.PageInstances.AsNoTracking().CountAsync(ct);
        var publishedPages = await db.PageInstances.AsNoTracking().CountAsync(p => p.Status == PageStatus.Published, ct);
        var totalViews     = await db.PageInstances.AsNoTracking().SumAsync(p => p.ViewCount, ct);
        var todayViews     = await db.PageViewLogs.AsNoTracking().CountAsync(l => l.ViewedAt >= today, ct);
        var totalAssets    = await db.FileAssets.AsNoTracking().CountAsync(ct);

        // User count via AspNetUsers table (ApplicationUser)
        var totalUsers = await db.Users.AsNoTracking().CountAsync(ct);

        return Result<DashboardStatsDto>.Success(new DashboardStatsDto(
            totalPages, publishedPages, totalViews, todayViews, totalAssets, totalUsers));
    }
}
