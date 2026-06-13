using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageInstances.Queries.GetPageInstanceById;

public record GetPageInstanceByIdQuery(Guid Id) : IRequest<Result<PageInstanceDto>>;
