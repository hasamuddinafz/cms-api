using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageModels.Commands.UpdatePageModel;

public record UpdatePageModelCommand(
    Guid Id,
    string Name,
    string? Description,
    string? Icon,
    bool IsSingleton,
    string SchemaJson
) : IRequest<Result>;
