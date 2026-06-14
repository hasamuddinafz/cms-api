using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Languages.Queries.GetLanguageList;

public class GetLanguageListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetLanguageListQuery, Result<List<LanguageDto>>>
{
    public async Task<Result<List<LanguageDto>>> Handle(GetLanguageListQuery request, CancellationToken cancellationToken)
    {
        var query = db.Languages.AsNoTracking().AsQueryable();

        if (request.ActiveOnly)
            query = query.Where(l => l.IsActive);

        var list = await query
            .OrderBy(l => l.SortOrder)
            .ThenBy(l => l.Name)
            .Select(l => new LanguageDto(l.Id, l.Code, l.Name, l.IsDefault, l.IsActive, l.FlagIcon, l.SortOrder))
            .ToListAsync(cancellationToken);

        return Result<List<LanguageDto>>.Success(list);
    }
}
