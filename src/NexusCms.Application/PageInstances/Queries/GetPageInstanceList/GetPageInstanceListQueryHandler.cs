using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.PageInstances.Queries.GetPageInstanceList;

public class GetPageInstanceListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPageInstanceListQuery, PagedResult<PageInstanceListItemDto>>
{
    public async Task<PagedResult<PageInstanceListItemDto>> Handle(GetPageInstanceListQuery request, CancellationToken ct)
    {
        var q = db.PageInstances.AsQueryable();

        // Optional: filter to a single model
        if (request.PageModelId.HasValue)
            q = q.Where(i => i.PageModelId == request.PageModelId.Value);

        // Model key filter
        if (request.Request.Filters.TryGetValue("modelKey", out var modelKey) && !string.IsNullOrWhiteSpace(modelKey))
            q = q.Where(i => i.PageModel.Key == modelKey);

        // Status filter
        if (request.Request.Filters.TryGetValue("status", out var statusStr)
            && Enum.TryParse<PageStatus>(statusStr, true, out var status))
            q = q.Where(i => i.Status == status);

        // Date range filters
        if (request.Request.Filters.TryGetValue("from", out var fromStr)
            && DateTime.TryParse(fromStr, out var from))
            q = q.Where(i => i.CreatedAt >= from);

        if (request.Request.Filters.TryGetValue("to", out var toStr)
            && DateTime.TryParse(toStr, out var to))
            q = q.Where(i => i.CreatedAt <= to);

        // Search
        if (!string.IsNullOrWhiteSpace(request.Request.Search))
        {
            var term = request.Request.Search.Trim().ToLower();
            q = q.Where(i => i.Slug.ToLower().Contains(term)
                           || i.PageModel.Name.ToLower().Contains(term));
        }

        // Sort
        q = (request.Request.SortBy?.ToLower(), request.Request.SortDir?.ToLower()) switch
        {
            ("slug",        "desc") => q.OrderByDescending(i => i.Slug),
            ("slug",        _)      => q.OrderBy(i => i.Slug),
            ("status",      "desc") => q.OrderByDescending(i => i.Status),
            ("status",      _)      => q.OrderBy(i => i.Status),
            ("viewcount",   "desc") => q.OrderByDescending(i => i.ViewCount),
            ("viewcount",   _)      => q.OrderBy(i => i.ViewCount),
            ("publishedat", "desc") => q.OrderByDescending(i => i.PublishedAt),
            ("publishedat", _)      => q.OrderBy(i => i.PublishedAt),
            ("createdat",   "desc") => q.OrderByDescending(i => i.CreatedAt),
            _                       => q.OrderByDescending(i => i.UpdatedAt)
        };

        var total = await q.CountAsync(ct);

        var items = await q
            .Skip(request.Request.Skip)
            .Take(request.Request.PageSize)
            .Select(i => new PageInstanceListItemDto(
                i.Id,
                i.PageModelId,
                i.PageModel.Key,
                i.PageModel.Name,
                i.Slug,
                i.Status,
                i.ViewCount,
                i.PublishedAt,
                i.CreatedAt,
                i.UpdatedAt))
            .ToListAsync(ct);

        return PagedResult<PageInstanceListItemDto>.Create(items, total, request.Request);
    }
}
