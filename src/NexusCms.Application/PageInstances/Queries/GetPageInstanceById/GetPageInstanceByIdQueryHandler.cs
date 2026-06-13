using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageInstances.Queries.GetPageInstanceById;

public class GetPageInstanceByIdQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPageInstanceByIdQuery, Result<PageInstanceDto>>
{
    public async Task<Result<PageInstanceDto>> Handle(GetPageInstanceByIdQuery request, CancellationToken ct)
    {
        var dto = await db.PageInstances
            .Where(i => i.Id == request.Id)
            .Select(i => new PageInstanceDto(
                i.Id,
                i.PageModelId,
                i.PageModel.Key,
                i.PageModel.Name,
                i.Slug,
                i.Status,
                i.ContentJson,
                i.SeoMetaJson,
                i.ViewCount,
                i.PublishedAt,
                i.CreatedByUserId,
                i.UpdatedByUserId,
                i.CreatedAt,
                i.UpdatedAt))
            .FirstOrDefaultAsync(ct);

        return dto is not null
            ? Result.Success(dto)
            : Result.Failure<PageInstanceDto>("Sayfa içeriği bulunamadı.");
    }
}
