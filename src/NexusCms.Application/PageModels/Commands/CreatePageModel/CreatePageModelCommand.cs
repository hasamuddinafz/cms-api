using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageModels.Commands.CreatePageModel;

public record CreatePageModelCommand(
    string Key,
    string Name,
    string? Description,
    string? Icon,
    bool IsSingleton,
    string SchemaJson
) : IRequest<Result<Guid>>;
