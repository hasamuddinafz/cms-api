using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageModels.Queries.GetPageModelById;

public record GetPageModelByIdQuery(Guid Id) : IRequest<Result<PageModelDto>>;
