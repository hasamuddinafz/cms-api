using NexusCms.Application.Common.Interfaces;
using MediatR;
using NexusCms.Domain.Common;


namespace NexusCms.Application.Subscribers.Commands.DeleteSubscriber;

public class DeleteSubscriberHandler(IDbContextAccessor db) : IRequestHandler<DeleteSubscriberCommand, Result>
{
    public async Task<Result> Handle(DeleteSubscriberCommand request, CancellationToken ct)
    {
        var sub = await db.Subscribers.FindAsync([request.Id], ct);
        if (sub is null) return Result.Failure("Abone bulunamadı.");
        db.Subscribers.Remove(sub);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
