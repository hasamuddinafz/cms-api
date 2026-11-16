using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;

namespace NexusCms.Application.PageModels.Commands.CreatePageModel;

public class CreatePageModelCommandHandler(IDbContextAccessor db)
    : IRequestHandler<CreatePageModelCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreatePageModelCommand request, CancellationToken ct)
    {
        var keyTaken = await db.PageModels.AnyAsync(m => m.Key == request.Key, ct);
        if (keyTaken)
            return Result.Failure<Guid>($"Key '{request.Key}' is already in use.");

        var model = new PageModel
        {
            Key         = request.Key,
            Name        = request.Name,
            Description = request.Description,
            Icon        = request.Icon,
            IsSingleton = request.IsSingleton,
            SchemaJson  = request.SchemaJson
        };

        await db.PageModels.AddAsync(model, ct);
        await db.SaveChangesAsync(ct);

        return Result.Success(model.Id);
    }
}
