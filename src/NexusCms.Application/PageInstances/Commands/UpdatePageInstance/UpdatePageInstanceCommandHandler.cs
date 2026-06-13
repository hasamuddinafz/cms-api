using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageInstances.Commands.UpdatePageInstance;

public class UpdatePageInstanceCommandHandler(IDbContextAccessor db, ICurrentUserService currentUser)
    : IRequestHandler<UpdatePageInstanceCommand, Result>
{
    public async Task<Result> Handle(UpdatePageInstanceCommand request, CancellationToken ct)
    {
        var instance = await db.PageInstances.FindAsync([request.Id], ct);
        if (instance is null)
            return Result.Failure("Sayfa içeriği bulunamadı.");

        // Slug uniqueness within same model (excluding self)
        var slugTaken = await db.PageInstances.AnyAsync(
            i => i.PageModelId == instance.PageModelId
              && i.Slug == request.Slug
              && i.Id != request.Id, ct);
        if (slugTaken)
            return Result.Failure("Bu model için aynı slug zaten kullanımda.");

        instance.Slug             = request.Slug;
        instance.ContentJson      = request.ContentJson;
        instance.SeoMetaJson      = request.SeoMetaJson;
        instance.UpdatedByUserId  = currentUser.UserId;

        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
