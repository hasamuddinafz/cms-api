using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageInstances.Commands.CreatePageInstance;

public record CreatePageInstanceCommand(
    Guid PageModelId,
    string Slug,
    string ContentJson,
    string SeoMetaJson
) : IRequest<Result<Guid>>;
