using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageModels.Commands.DeletePageModel;

public class DeletePageModelCommandHandler(IDbContextAccessor db)
    : IRequestHandler<DeletePageModelCommand, Result>
{
    public async Task<Result> Handle(DeletePageModelCommand request, CancellationToken ct)
    {
        var model = await db.PageModels.FindAsync([request.Id], ct);
        if (model is null)
            return Result.Failure("Page model not found.");

        var hasInstances = await db.PageInstances.AnyAsync(i => i.PageModelId == request.Id, ct);
        if (hasInstances)
            return Result.Failure("This model has content items attached. Delete them first.");

        db.PageModels.Remove(model);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
