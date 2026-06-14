using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Models;

namespace NexusCms.Application.PageModels.Queries.GetPageModelList;

public class GetPageModelListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPageModelListQuery, PagedResult<PageModelDto>>
{
    public async Task<PagedResult<PageModelDto>> Handle(GetPageModelListQuery request, CancellationToken ct)
    {
        var q = db.PageModels.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Request.Search))
        {
            var term = request.Request.Search.Trim().ToLower();
            q = q.Where(m => m.Name.ToLower().Contains(term) || m.Key.ToLower().Contains(term));
        }

        if (request.Request.Filters.TryGetValue("isSingleton", out var singleton))
        {
            var val = bool.Parse(singleton);
            q = q.Where(m => m.IsSingleton == val);
        }

        q = (request.Request.SortBy?.ToLower(), request.Request.SortDir?.ToLower()) switch
        {
            ("name", "desc")       => q.OrderByDescending(m => m.Name),
            ("name", _)            => q.OrderBy(m => m.Name),
            ("key", "desc")        => q.OrderByDescending(m => m.Key),
            ("key", _)             => q.OrderBy(m => m.Key),
            ("createdat", "desc")  => q.OrderByDescending(m => m.CreatedAt),
            ("createdat", _)       => q.OrderBy(m => m.CreatedAt),
            _                      => q.OrderBy(m => m.Name)
        };

        var total = await q.CountAsync(ct);

        var items = await q
            .Skip(request.Request.Skip)
            .Take(request.Request.PageSize)
            .Select(m => new PageModelDto(
                m.Id, m.Key, m.Name, m.Description, m.Icon, m.IsSingleton, m.SchemaJson,
                m.PageInstances.Count,
                m.CreatedAt, m.UpdatedAt))
            .ToListAsync(ct);

        return PagedResult<PageModelDto>.Create(items, total, request.Request);
    }
}
