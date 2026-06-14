using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.Public.Queries.GetPublicPage;

public class GetPublicPageQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPublicPageQuery, Result<PublicPageDto>>
{
    public async Task<Result<PublicPageDto>> Handle(GetPublicPageQuery request, CancellationToken ct)
    {
        var page = await db.PageInstances
            .AsNoTracking()
            .Where(p => p.PageModel.Key == request.ModelKey
                     && p.Slug         == request.Slug
                     && p.Status       == PageStatus.Published)
            .Select(p => new PublicPageDto(
                p.Id,
                p.PageModel.Key,
                p.Slug,
                p.ContentJson,
                p.SeoMetaJson,
                p.ViewCount,
                p.PublishedAt))
            .FirstOrDefaultAsync(ct);

        return page is not null
            ? Result<PublicPageDto>.Success(page)
            : Result<PublicPageDto>.Failure("Sayfa bulunamadı veya yayınlanmamış.");
    }
}
