using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusCms.Application.Common.Interfaces;
using NexusCms.Application.Public;
using NexusCms.Application.Public.Queries.GetPublicPage;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that fires only for <see cref="GetPublicPageQuery"/>.
/// On success it:
///   1. Atomically increments PageInstance.ViewCount via a single UPDATE statement.
///   2. Enqueues a PageViewLog entry to the background writer (fire-and-forget).
/// </summary>
public class PageViewTrackingBehavior(
    IDbContextAccessor db,
    IPageViewQueue     queue,
    ICurrentUserService currentUser)
    : IPipelineBehavior<GetPublicPageQuery, Result<PublicPageDto>>
{
    public async Task<Result<PublicPageDto>> Handle(
        GetPublicPageQuery            request,
        RequestHandlerDelegate<Result<PublicPageDto>> next,
        CancellationToken             cancellationToken)
    {
        var result = await next();

        if (result.IsSuccess && result.Value is { } page)
        {
            // 1. Atomic ViewCount increment — bypasses change tracker, no race condition
            await db.PageInstances
                .Where(p => p.Id == page.Id)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(p => p.ViewCount, p => p.ViewCount + 1),
                    cancellationToken);

            // 2. Enqueue async PageViewLog write (fire-and-forget via IHostedService)
            queue.Enqueue(new PageViewLogEntry(
                page.Id,
                IpHash:    currentUser.IpAddress ?? string.Empty,
                UserAgent: currentUser.UserAgent  ?? string.Empty,
                Referrer:  currentUser.Referrer   ?? string.Empty));
        }

        return result;
    }
}
