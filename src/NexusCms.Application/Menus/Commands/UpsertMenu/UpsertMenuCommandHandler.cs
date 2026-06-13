using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.Menus.Commands.UpsertMenu;

public class UpsertMenuCommandHandler(IDbContextAccessor db)
    : IRequestHandler<UpsertMenuCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(UpsertMenuCommand request, CancellationToken ct)
    {
        var existing = await db.Menus.FirstOrDefaultAsync(m => m.Key == request.Key, ct);

        if (existing is not null)
        {
            existing.ItemsJson = request.ItemsJson;
            existing.Type      = request.Type;
            await db.SaveChangesAsync(ct);
            return Result.Success(existing.Id);
        }

        var menu = new Menu
        {
            Key       = request.Key,
            Type      = request.Type,
            ItemsJson = request.ItemsJson
        };

        await db.Menus.AddAsync(menu, ct);
        await db.SaveChangesAsync(ct);
        return Result.Success(menu.Id);
    }
}
