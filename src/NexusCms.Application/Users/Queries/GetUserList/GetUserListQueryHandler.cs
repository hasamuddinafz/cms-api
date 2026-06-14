using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Users.Queries.GetUserList;

public class GetUserListQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetUserListQuery, Result<PagedResult<UserListItemDto>>>
{
    public async Task<Result<PagedResult<UserListItemDto>>> Handle(GetUserListQuery request, CancellationToken ct)
    {
        var query = db.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(u =>
                (u.Email != null && u.Email.Contains(request.Search)) ||
                (u.FullName != null && u.FullName.Contains(request.Search)));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(u => u.Email)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserListItemDto(
                u.Id,
                u.Email,
                u.FullName,
                u.IsActive,
                u.LastLoginAt,
                u.UserRoles.Select(ur => ur.Role.Name!).ToList()))
            .ToListAsync(ct);

        return Result<PagedResult<UserListItemDto>>.Success(
            PagedResult<UserListItemDto>.Create(items, total, request.Page, request.PageSize));
    }
}
