using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;

namespace NexusCms.Application.Menus.Queries.GetMenuList;

public class GetMenuListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetMenuListQuery, IReadOnlyList<MenuDto>>
{
    public async Task<IReadOnlyList<MenuDto>> Handle(GetMenuListQuery request, CancellationToken ct)
        => await db.Menus
            .AsNoTracking()
            .OrderBy(m => m.Key)
            .Select(m => new MenuDto(m.Id, m.Key, m.Type, m.ItemsJson, m.CreatedAt, m.UpdatedAt))
            .ToListAsync(ct);
}
