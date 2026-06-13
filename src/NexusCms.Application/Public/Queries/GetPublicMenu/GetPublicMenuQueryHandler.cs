using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicMenu;

public class GetPublicMenuQueryHandler(IDbContextAccessor db)
    : IRequestHandler<GetPublicMenuQuery, Result<PublicMenuDto>>
{
    public async Task<Result<PublicMenuDto>> Handle(GetPublicMenuQuery request, CancellationToken ct)
    {
        var menu = await db.Menus
            .Where(m => m.Key == request.Key)
            .Select(m => new PublicMenuDto(m.Key, m.ItemsJson))
            .FirstOrDefaultAsync(ct);

        return menu is not null
            ? Result<PublicMenuDto>.Success(menu)
            : Result<PublicMenuDto>.Failure($"Menü bulunamadı: {request.Key}");
    }
}
