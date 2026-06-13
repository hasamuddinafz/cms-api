using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageModels.Commands.DeletePageModel;

public record DeletePageModelCommand(Guid Id) : IRequest<Result>;
