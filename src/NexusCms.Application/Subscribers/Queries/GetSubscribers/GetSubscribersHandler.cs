using NexusCms.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Models;


namespace NexusCms.Application.Subscribers.Queries.GetSubscribers;

public class GetSubscribersHandler(IDbContextAccessor db) : IRequestHandler<GetSubscribersQuery, PagedResult<SubscriberDto>>
{
    public async Task<PagedResult<SubscriberDto>> Handle(GetSubscribersQuery request, CancellationToken ct)
    {
        var query = db.Subscribers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(x => x.Email.ToLower().Contains(s) || (x.FullName != null && x.FullName.ToLower().Contains(s)));
        }

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        query = request.SortBy.ToLower() switch
        {
            "email"    => request.SortDir == "asc" ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email),
            "fullname" => request.SortDir == "asc" ? query.OrderBy(x => x.FullName) : query.OrderByDescending(x => x.FullName),
            _          => request.SortDir == "asc" ? query.OrderBy(x => x.SubscribedAt) : query.OrderByDescending(x => x.SubscribedAt),
        };

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new SubscriberDto(x.Id, x.Email, x.FullName, x.IsActive, x.SubscribedAt, x.UnsubscribedAt))
            .ToListAsync(ct);

        return PagedResult<SubscriberDto>.Create(items, total, request.Page, request.PageSize);
    }
}
