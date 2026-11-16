using NexusCms.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Domain.Common;


namespace NexusCms.Application.Subscribers.Commands.Unsubscribe;

public class UnsubscribeHandler(IDbContextAccessor db) : IRequestHandler<UnsubscribeCommand, Result>
{
    public async Task<Result> Handle(UnsubscribeCommand request, CancellationToken ct)
    {
        var sub = await db.Subscribers.FirstOrDefaultAsync(x => x.Email == request.Email, ct);
        if (sub is null) return Result.Failure("Subscriber not found.");
        sub.IsActive = false;
        sub.UnsubscribedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
