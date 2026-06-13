using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Assets.Commands.DeleteFile;

public record DeleteFileCommand(Guid Id) : IRequest<Result>;
