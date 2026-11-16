using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageInstances.Commands.DeletePageInstance;

public class DeletePageInstanceCommandHandler(IDbContextAccessor db)
    : IRequestHandler<DeletePageInstanceCommand, Result>
{
    public async Task<Result> Handle(DeletePageInstanceCommand request, CancellationToken ct)
    {
        var instance = await db.PageInstances.FindAsync([request.Id], ct);
        if (instance is null)
            return Result.Failure("Page content not found.");

        db.PageInstances.Remove(instance);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
