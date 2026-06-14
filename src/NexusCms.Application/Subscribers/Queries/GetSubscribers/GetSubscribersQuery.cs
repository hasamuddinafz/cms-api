using MediatR;
using NexusCms.Application.Common.Models;

namespace NexusCms.Application.Subscribers.Queries.GetSubscribers;

public record GetSubscribersQuery(
    string? Search,
    bool? IsActive,
    string SortBy = "subscribedAt",
    string SortDir = "desc",
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResult<SubscriberDto>>;

public record SubscriberDto(
    Guid Id,
    string Email,
    string? FullName,
    bool IsActive,
    DateTime SubscribedAt,
    DateTime? UnsubscribedAt
);
