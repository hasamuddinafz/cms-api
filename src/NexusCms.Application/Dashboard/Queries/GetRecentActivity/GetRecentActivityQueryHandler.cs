using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Dashboard.Queries.GetRecentActivity;

public class GetRecentActivityQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetRecentActivityQuery, Result<List<RecentActivityDto>>>
{
    public async Task<Result<List<RecentActivityDto>>> Handle(GetRecentActivityQuery request, CancellationToken ct)
    {
        var logs = await db.AuditLogs
            .OrderByDescending(l => l.CreatedAt)
            .Take(request.Take)
            .GroupJoin(
                db.Users,
                l => l.UserId,
                u => u.Id,
                (l, users) => new { l, users })
            .SelectMany(
                x => x.users.DefaultIfEmpty(),
                (x, u) => new RecentActivityDto(
                    x.l.Id,
                    x.l.Action.ToString(),
                    x.l.EntityType,
                    x.l.EntityId,
                    u != null ? u.Email : null,
                    x.l.CreatedAt))
            .ToListAsync(ct);

        return Result<List<RecentActivityDto>>.Success(logs);
    }
}
