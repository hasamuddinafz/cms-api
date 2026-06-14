using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Dashboard.Queries.GetTopPages;

public class GetTopPagesQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetTopPagesQuery, Result<List<TopPageDto>>>
{
    public async Task<Result<List<TopPageDto>>> Handle(GetTopPagesQuery request, CancellationToken ct)
    {
        var pages = await db.PageInstances
            .AsNoTracking()
            .Where(p => p.Status == PageStatus.Published)
            .OrderByDescending(p => p.ViewCount)
            .Take(request.Take)
            .Select(p => new TopPageDto(p.Id, p.Slug, p.PageModel.Key, p.ViewCount))
            .ToListAsync(ct);

        return Result<List<TopPageDto>>.Success(pages);
    }
}
