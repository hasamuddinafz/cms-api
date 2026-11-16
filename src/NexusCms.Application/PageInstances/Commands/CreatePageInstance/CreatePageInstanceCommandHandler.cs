using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.PageInstances.Commands.CreatePageInstance;

public class CreatePageInstanceCommandHandler(IDbContextAccessor db, ICurrentUserService currentUser)
    : IRequestHandler<CreatePageInstanceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreatePageInstanceCommand request, CancellationToken ct)
    {
        var model = await db.PageModels.FindAsync([request.PageModelId], ct);
        if (model is null)
            return Result.Failure<Guid>("Page model not found.");

        // Singleton check: only one instance allowed
        if (model.IsSingleton)
        {
            var exists = await db.PageInstances.AnyAsync(i => i.PageModelId == request.PageModelId, ct);
            if (exists)
                return Result.Failure<Guid>($"'{model.Name}' is a singleton model, so only one content item can be created.");
        }

        var slugTaken = await db.PageInstances
            .AnyAsync(i => i.PageModelId == request.PageModelId && i.Slug == request.Slug, ct);
        if (slugTaken)
            return Result.Failure<Guid>("This slug is already in use for this model.");

        var instance = new PageInstance
        {
            PageModelId      = request.PageModelId,
            Slug             = request.Slug,
            ContentJson      = request.ContentJson,
            SeoMetaJson      = request.SeoMetaJson,
            CreatedByUserId  = currentUser.UserId,
            UpdatedByUserId  = currentUser.UserId
        };

        await db.PageInstances.AddAsync(instance, ct);
        await db.SaveChangesAsync(ct);
        return Result.Success(instance.Id);
    }
}
