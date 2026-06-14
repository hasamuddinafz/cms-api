using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Models;
using NexusCms.Application.MailTemplates;
using NexusCms.Domain.Common;

namespace NexusCms.Application.MailTemplates.Queries.GetMailTemplateList;

public class GetMailTemplateListHandler(IDbContextAccessor db)
    : IRequestHandler<GetMailTemplateListQuery, Result<PagedResult<MailTemplateListItemDto>>>
{
    public async Task<Result<PagedResult<MailTemplateListItemDto>>> Handle(GetMailTemplateListQuery request, CancellationToken ct)
    {
        var query = db.MailTemplates.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(t =>
                t.Name.Contains(request.Search) ||
                t.Subject.Contains(request.Search));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new MailTemplateListItemDto(t.Id, t.Name, t.Subject, t.CreatedAt))
            .ToListAsync(ct);

        return Result<PagedResult<MailTemplateListItemDto>>.Success(
            PagedResult<MailTemplateListItemDto>.Create(items, total, request.Page, request.PageSize));
    }
}
