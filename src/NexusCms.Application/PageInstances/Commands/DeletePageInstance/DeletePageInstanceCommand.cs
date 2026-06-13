using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.PageInstances.Commands.DeletePageInstance;

public record DeletePageInstanceCommand(Guid Id) : IRequest<Result>;
