using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageInstances.Commands.UpdatePageInstance;

public record UpdatePageInstanceCommand(
    Guid Id,
    string Slug,
    string ContentJson,
    string SeoMetaJson
) : IRequest<Result>;
