using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Public.Queries.GetPublicPageList;

public class GetPublicPageListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPublicPageListQuery, Result<PagedResult<PublicPageListItemDto>>>
{
    public async Task<Result<PagedResult<PublicPageListItemDto>>> Handle(GetPublicPageListQuery request, CancellationToken ct)
    {
        var query = db.PageInstances
            .AsNoTracking()
            .Where(p => p.PageModel.Key == request.ModelKey
                     && p.Status        == PageStatus.Published)
            .OrderByDescending(p => p.PublishedAt);

        var total = await query.CountAsync(ct);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PublicPageListItemDto(
                p.Id,
                p.Slug,
                p.SeoMetaJson,
                p.ViewCount,
                p.PublishedAt))
            .ToListAsync(ct);

        return Result<PagedResult<PublicPageListItemDto>>.Success(
            PagedResult<PublicPageListItemDto>.Create(items, total, request.Page, request.PageSize));
    }
}
