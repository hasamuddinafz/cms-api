using MediatR;
using NexusCms.Application.Common.Models;

namespace NexusCms.Application.PageInstances.Queries.GetPageInstanceList;

public record GetPageInstanceListQuery(PagedRequest Request, Guid? PageModelId = null)
    : IRequest<PagedResult<PageInstanceListItemDto>>;
