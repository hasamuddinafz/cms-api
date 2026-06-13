using MediatR;
using NexusCms.Application.Common.Models;
using NexusCms.Application.PageModels.Queries.ExportPageModels;

namespace NexusCms.Application.PageInstances.Queries.ExportPageInstances;

public record ExportPageInstancesQuery(PagedRequest Request, string Format, Guid? PageModelId = null)
    : IRequest<ExportResult>;
