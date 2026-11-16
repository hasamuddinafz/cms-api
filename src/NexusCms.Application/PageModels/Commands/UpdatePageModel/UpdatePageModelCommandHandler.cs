using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageModels.Commands.UpdatePageModel;

public class UpdatePageModelCommandHandler(IDbContextAccessor db)
    : IRequestHandler<UpdatePageModelCommand, Result>
{
    public async Task<Result> Handle(UpdatePageModelCommand request, CancellationToken ct)
    {
        var model = await db.PageModels.FindAsync([request.Id], ct);
        if (model is null)
            return Result.Failure("Page model not found.");

        model.Name        = request.Name;
        model.Description = request.Description;
        model.Icon        = request.Icon;
        model.IsSingleton = request.IsSingleton;
        model.SchemaJson  = request.SchemaJson;

        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
