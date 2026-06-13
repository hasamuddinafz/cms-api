using MediatR;
using NexusCms.Domain.Common;

namespace NexusCms.Application.Languages.Commands.DeleteLanguage;

public record DeleteLanguageCommand(Guid Id) : IRequest<Result>;
