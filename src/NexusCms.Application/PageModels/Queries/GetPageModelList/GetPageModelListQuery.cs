using MediatR;
using NexusCms.Application.Common.Models;

namespace NexusCms.Application.PageModels.Queries.GetPageModelList;

public record GetPageModelListQuery(PagedRequest Request) : IRequest<PagedResult<PageModelDto>>;
