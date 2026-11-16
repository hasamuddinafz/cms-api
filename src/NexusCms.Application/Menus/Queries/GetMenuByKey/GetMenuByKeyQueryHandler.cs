using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Menus.Queries.GetMenuByKey;

public class GetMenuByKeyQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetMenuByKeyQuery, Result<MenuDto>>
{
    public async Task<Result<MenuDto>> Handle(GetMenuByKeyQuery request, CancellationToken ct)
    {
        var dto = await db.Menus
            .AsNoTracking()
            .Where(m => m.Key == request.Key)
            .Select(m => new MenuDto(m.Id, m.Key, m.Type, m.ItemsJson, m.CreatedAt, m.UpdatedAt))
            .FirstOrDefaultAsync(ct);

        return dto is not null
            ? Result.Success(dto)
            : Result.Failure<MenuDto>($"Menu with key '{request.Key}' not found.");
    }
}
