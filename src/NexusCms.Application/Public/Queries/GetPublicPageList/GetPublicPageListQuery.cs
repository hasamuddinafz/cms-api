using MediatR;
using NexusCms.Application.Common.Models;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Public.Queries.GetPublicPageList;

public record GetPublicPageListQuery(
    string ModelKey,
    int    Page     = 1,
    int    PageSize = 10) : IRequest<Result<PagedResult<PublicPageListItemDto>>>;
