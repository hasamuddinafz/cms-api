using MediatR;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Domain.Common;
using NexusCms.Domain.Enums;

namespace NexusCms.Application.PageInstances.Commands.ChangePageInstanceStatus;

public class ChangePageInstanceStatusCommandHandler(IDbContextAccessor db, ICurrentUserService currentUser)
    : IRequestHandler<ChangePageInstanceStatusCommand, Result>
{
    public async Task<Result> Handle(ChangePageInstanceStatusCommand request, CancellationToken ct)
    {
        var instance = await db.PageInstances.FindAsync([request.Id], ct);
        if (instance is null)
            return Result.Failure("Sayfa içeriği bulunamadı.");

        var valid = (instance.Status, request.TargetStatus) switch
        {
            (PageStatus.Draft,     PageStatus.Published) => true,
            (PageStatus.Published, PageStatus.Archived)  => true,
            (PageStatus.Archived,  PageStatus.Draft)     => true,   // un-archive back to draft
            _ => false
        };

        if (!valid)
            return Result.Failure($"'{instance.Status}' durumundan '{request.TargetStatus}' durumuna geçiş yapılamaz.");

        instance.Status           = request.TargetStatus;
        instance.UpdatedByUserId  = currentUser.UserId;

        if (request.TargetStatus == PageStatus.Published)
            instance.PublishedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
