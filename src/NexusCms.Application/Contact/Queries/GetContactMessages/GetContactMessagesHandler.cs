using NexusCms.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Models;


namespace NexusCms.Application.Contact.Queries.GetContactMessages;

public class GetContactMessagesHandler(IDbContextAccessor db) : IRequestHandler<GetContactMessagesQuery, PagedResult<ContactMessageDto>>
{
    public async Task<PagedResult<ContactMessageDto>> Handle(GetContactMessagesQuery request, CancellationToken ct)
    {
        var query = db.ContactMessages.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var s = request.Search.ToLower();
            query = query.Where(x =>
                x.FullName.ToLower().Contains(s) ||
                x.Email.ToLower().Contains(s) ||
                x.Subject.ToLower().Contains(s));
        }

        if (request.IsRead.HasValue)
            query = query.Where(x => x.IsRead == request.IsRead.Value);

        query = request.SortBy.ToLower() switch
        {
            "fullname" => request.SortDir == "asc" ? query.OrderBy(x => x.FullName) : query.OrderByDescending(x => x.FullName),
            "email"    => request.SortDir == "asc" ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email),
            _          => request.SortDir == "asc" ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt),
        };

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ContactMessageDto(x.Id, x.FullName, x.Email, x.Phone, x.Subject, x.Message, x.IsRead, x.IpAddress, x.CreatedAt))
            .ToListAsync(ct);

        return PagedResult<ContactMessageDto>.Create(items, total, request.Page, request.PageSize);
    }
}
