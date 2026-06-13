using MediatR;
using NexusCms.Application.Common.Models;
using NexusCms.Application.PageModels.Queries.ExportPageModels;

namespace NexusCms.Application.Assets.Queries.ExportFiles;

public record ExportFilesQuery(PagedRequest Request, string Format) : IRequest<ExportResult>;
