using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Roles.Queries.GetRoleList;

public class GetRoleListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetRoleListQuery, Result<PagedResult<RoleListItemDto>>>
{
    public async Task<Result<PagedResult<RoleListItemDto>>> Handle(GetRoleListQuery request, CancellationToken ct)
    {
        var query = db.Roles.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(r => r.Name!.Contains(request.Search));

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(r => r.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new RoleListItemDto(
                r.Id,
                r.Name!,
                r.Description,
                r.IsSystemRole,
                r.RolePermissions.Count,
                r.CreatedAt))
            .ToListAsync(ct);

        return Result<PagedResult<RoleListItemDto>>.Success(
            PagedResult<RoleListItemDto>.Create(items, total, request.Page, request.PageSize));
    }
}
