using MediatR;
using NexusCms.Application.Common.Models;

namespace NexusCms.Application.PageModels.Queries.ExportPageModels;

public record ExportPageModelsQuery(PagedRequest Request, string Format) : IRequest<ExportResult>;

public record ExportResult(byte[] Content, string ContentType, string FileName);
