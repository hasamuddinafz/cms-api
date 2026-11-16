using NexusCms.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Domain.Common;
using NexusCms.Domain.Entities;


namespace NexusCms.Application.Subscribers.Commands.Subscribe;

public class SubscribeHandler(IDbContextAccessor db) : IRequestHandler<SubscribeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SubscribeCommand request, CancellationToken ct)
    {
        var existing = await db.Subscribers.FirstOrDefaultAsync(x => x.Email == request.Email, ct);
        if (existing is not null)
        {
            if (existing.IsActive) return Result.Failure<Guid>("This email is already subscribed.");
            existing.IsActive = true;
            existing.UnsubscribedAt = null;
            await db.SaveChangesAsync(ct);
            return Result.Success(existing.Id);
        }

        var sub = new Subscriber
        {
            Email     = request.Email,
            FullName  = request.FullName,
            IpAddress = request.IpAddress,
        };
        db.Subscribers.Add(sub);
        await db.SaveChangesAsync(ct);
        return Result.Success(sub.Id);
    }
}
