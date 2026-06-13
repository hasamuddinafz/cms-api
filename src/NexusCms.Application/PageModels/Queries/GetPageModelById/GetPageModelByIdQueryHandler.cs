using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageModels.Queries.GetPageModelById;

public class GetPageModelByIdQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPageModelByIdQuery, Result<PageModelDto>>
{
    public async Task<Result<PageModelDto>> Handle(GetPageModelByIdQuery request, CancellationToken ct)
    {
        var dto = await db.PageModels
            .Where(m => m.Id == request.Id)
            .Select(m => new PageModelDto(
                m.Id, m.Key, m.Name, m.Description, m.Icon, m.IsSingleton, m.SchemaJson,
                m.PageInstances.Count,
                m.CreatedAt, m.UpdatedAt))
            .FirstOrDefaultAsync(ct);

        return dto is not null
            ? Result.Success(dto)
            : Result.Failure<PageModelDto>("Sayfa modeli bulunamadı.");
    }
}
